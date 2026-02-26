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
                    SellerName = b.Seller.FullName,
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
                SellerName = bike.Seller.FullName,
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
                CreatedAt = bike.CreatedAt,
                ImageUrls = bike.BikeImages.Select(bi => bi.ImageUrl).ToList()
            };
        }

        public async Task<BikeResponseDto> CreateAsync(CreateBikeDto dto)
        {
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
                Status = BikeStatus.PENDING,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(bike.BikeId) ?? throw new Exception("Failed to create bike");
        }

        public async Task<bool> UpdateAsync(int id, UpdateBikeDto dto)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null) return false;

            bike.Title = dto.Title;
            bike.Description = dto.Description;
            bike.Price = dto.Price;
            bike.BrandId = dto.BrandId;
            bike.CategoryId = dto.CategoryId;
            bike.FrameSize = dto.FrameSize;

            if (dto.BikeCondition != null && Enum.TryParse<BikeCondition>(dto.BikeCondition, out var condition))
                bike.BikeCondition = condition;

            if (dto.Status != null && Enum.TryParse<BikeStatus>(dto.Status, out var status))
                bike.Status = status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null) return false;

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
