using Backend_CycleTrust.BLL.DTOs.BikeDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class BikeService : IBikeService
    {
        private readonly CycleTrustDbContext _context;

        public BikeService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BikeResponseDto>> GetAllAsync()
        {
            return await _context.Bikes
                .Include(b => b.Seller)
                .Include(b => b.Brand)
                .Include(b => b.Category)
                .Include(b => b.BikeImages)
                .Select(b => new BikeResponseDto
                {
                    BikeId = b.BikeId,
                    SellerId = b.SellerId,
                    SellerName = b.IsAnonymous ? "Anonymous" : b.Seller.FullName,
                    Title = b.Title,
                    Description = b.Description,
                    Price = b.Price,
                    BrandId = b.BrandId,
                    BrandName = b.Brand != null ? b.Brand.BrandName : null,
                    CategoryId = b.CategoryId,
                    CategoryName = b.Category != null ? b.Category.CategoryName : null,
                    FrameSize = b.FrameSize,
                    BikeCondition = b.BikeCondition != null ? b.BikeCondition.ToString() : null,
                    Status = b.Status.ToString(),
                    IsAnonymous = b.IsAnonymous,
                    CreatedAt = b.CreatedAt,
                    ImageUrls = b.BikeImages.Select(bi => bi.ImageUrl).ToList()
                })
                .ToListAsync();
        }

        public async Task<BikeResponseDto?> GetByIdAsync(int id)
        {
            var bike = await _context.Bikes
                .Include(b => b.Seller)
                .Include(b => b.Brand)
                .Include(b => b.Category)
                .Include(b => b.BikeImages)
                .FirstOrDefaultAsync(b => b.BikeId == id);

            if (bike == null) return null;

            return new BikeResponseDto
            {
                BikeId = bike.BikeId,
                SellerId = bike.SellerId,
                SellerName = bike.IsAnonymous ? "Anonymous" : bike.Seller.FullName,
                Title = bike.Title,
                Description = bike.Description,
                Price = bike.Price,
                BrandId = bike.BrandId,
                BrandName = bike.Brand?.BrandName,
                CategoryId = bike.CategoryId,
                CategoryName = bike.Category?.CategoryName,
                FrameSize = bike.FrameSize,
                BikeCondition = bike.BikeCondition?.ToString(),
                Status = bike.Status.ToString(),
                IsAnonymous = bike.IsAnonymous,
                CreatedAt = bike.CreatedAt,
                ImageUrls = bike.BikeImages.Select(bi => bi.ImageUrl).ToList()
            };
        }

        public async Task<BikeResponseDto> CreateAsync(CreateBikeDto dto)
        {
            Console.WriteLine($"DEBUG: Creating bike for SellerId={dto.SellerId}, BrandId={dto.BrandId}, CategoryId={dto.CategoryId}");

            var bike = new Bike
            {
                SellerId = dto.SellerId,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
                FrameSize = dto.FrameSize,
                BikeCondition = dto.BikeCondition != null
                    ? Enum.Parse<BikeCondition>(dto.BikeCondition)
                    : null,
                IsAnonymous = dto.IsAnonymous,
                Status = BikeStatus.PENDING,
                CreatedAt = DateTime.UtcNow
            };

            // 1. Save Bike first to get the ID (SQLite works better this way for FKs)
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            // 2. Add External Images if any
            if (dto.ImageUrls != null && dto.ImageUrls.Any())
            {
                foreach (var url in dto.ImageUrls)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        bike.BikeImages.Add(new BikeImage 
                        { 
                            BikeId = bike.BikeId, 
                            ImageUrl = url 
                        });
                    }
                }
            }

            // 3. Add Uploaded Image Files if any
            if (dto.ImageFiles != null && dto.ImageFiles.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        bike.BikeImages.Add(new BikeImage 
                        { 
                            BikeId = bike.BikeId, 
                            ImageUrl = $"/uploads/{fileName}" 
                        });
                    }
                }
            }

            if ((dto.ImageUrls != null && dto.ImageUrls.Any()) || (dto.ImageFiles != null && dto.ImageFiles.Any()))
            {
                await _context.SaveChangesAsync();
            }

            return await GetByIdAsync(bike.BikeId) ?? throw new Exception("Failed to create bike");
        }

        public async Task<bool> UpdateAsync(int id, UpdateBikeDto dto)
        {
            Console.WriteLine($"DEBUG: Updating bike ID={id}");

            var bike = await _context.Bikes
                .Include(b => b.BikeImages)
                .FirstOrDefaultAsync(b => b.BikeId == id);
            if (bike == null) return false;

            bike.Title = dto.Title;
            bike.Description = dto.Description;
            bike.Price = dto.Price;
            bike.BrandId = dto.BrandId;
            bike.CategoryId = dto.CategoryId;
            bike.FrameSize = dto.FrameSize;
            bike.IsAnonymous = dto.IsAnonymous;

            if (dto.BikeCondition != null && Enum.TryParse<BikeCondition>(dto.BikeCondition, out var condition))
                bike.BikeCondition = condition;

            if (dto.Status != null && Enum.TryParse<BikeStatus>(dto.Status, out var status))
                bike.Status = status;

            // Handle External Images
            _context.BikeImages.RemoveRange(bike.BikeImages);
            if (dto.ImageUrls != null)
            {
                foreach (var url in dto.ImageUrls)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        bike.BikeImages.Add(new BikeImage { ImageUrl = url });
                    }
                }
            }

            // Handle Uploaded File Images
            if (dto.ImageFiles != null && dto.ImageFiles.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        bike.BikeImages.Add(new BikeImage { ImageUrl = $"/uploads/{fileName}" });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bike = await _context.Bikes
                .Include(b => b.BikeImages)
                .Include(b => b.Orders).ThenInclude(o => o.Reviews)
                .Include(b => b.Wishlists)
                .Include(b => b.InspectionReports)
                .Include(b => b.Messages)
                .Include(b => b.Reports)
                .FirstOrDefaultAsync(b => b.BikeId == id);
                
            if (bike == null) return false;

            // Delete associated physical image files
            if (bike.BikeImages != null && bike.BikeImages.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                foreach (var image in bike.BikeImages)
                {
                    if (!string.IsNullOrEmpty(image.ImageUrl) && image.ImageUrl.StartsWith("/uploads/"))
                    {
                        var fileName = image.ImageUrl.Replace("/uploads/", "");
                        var filePath = Path.Combine(uploadsFolder, fileName);
                        if (File.Exists(filePath))
                        {
                            try 
                            {
                                File.Delete(filePath);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"DEBUG: Failed to delete image file {filePath}: {ex.Message}");
                            }
                        }
                    }
                }
            }

            // Remove related entities due to Restrict/SetNull constraints
            if (bike.Orders != null && bike.Orders.Any())
            {
                foreach(var order in bike.Orders)
                {
                    if(order.Reviews != null && order.Reviews.Any())
                    {
                         _context.Reviews.RemoveRange(order.Reviews);
                    }
                }
                _context.Orders.RemoveRange(bike.Orders);
            }
            if (bike.Wishlists != null && bike.Wishlists.Any())
                _context.Wishlists.RemoveRange(bike.Wishlists);
            if (bike.InspectionReports != null && bike.InspectionReports.Any())
                _context.InspectionReports.RemoveRange(bike.InspectionReports);
            if (bike.Messages != null && bike.Messages.Any())
                _context.Messages.RemoveRange(bike.Messages);
            if (bike.Reports != null && bike.Reports.Any())
                _context.Reports.RemoveRange(bike.Reports);

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
