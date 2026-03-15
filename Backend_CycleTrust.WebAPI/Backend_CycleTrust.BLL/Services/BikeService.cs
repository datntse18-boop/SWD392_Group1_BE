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

        private static bool TryParseBikeCondition(string? value, out BikeCondition condition)
        {
            condition = default;
            if (string.IsNullOrWhiteSpace(value)) return false;

            var normalized = value.Trim().Replace("-", " ").Replace("_", " ").ToLowerInvariant();

            return normalized switch
            {
                "new" => (condition = BikeCondition.NEW) == BikeCondition.NEW,
                "like new" or "used like new" => (condition = BikeCondition.USED_LIKE_NEW) == BikeCondition.USED_LIKE_NEW,
                "used" or "used good" or "good" => (condition = BikeCondition.USED_GOOD) == BikeCondition.USED_GOOD,
                "needs repair" or "used fair" or "fair" => (condition = BikeCondition.USED_FAIR) == BikeCondition.USED_FAIR,
                _ => Enum.TryParse(value, ignoreCase: true, out condition)
            };
        }

        private static BikeResponseDto MapBikeToResponse(Bike bike)
        {
            var reports = bike.InspectionReports
                .OrderByDescending(r => r.InspectedAt)
                .Select(r => new BikeInspectionReportDto
                {
                    ReportId = r.ReportId,
                    InspectorId = r.InspectorId,
                    InspectorName = r.Inspector?.FullName,
                    ReportFile = r.ReportFile,
                    InspectionStatus = r.InspectionStatus.ToString(),
                    OverallComment = r.OverallComment,
                    InspectedAt = r.InspectedAt
                })
                .ToList();

            var latestReport = reports.FirstOrDefault();
            var isInspected = latestReport?.InspectionStatus == InspectionStatus.APPROVED.ToString();

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
                IsInspected = isInspected,
                InspectionStatus = latestReport?.InspectionStatus ?? "NOT_INSPECTED",
                InspectionWarning = isInspected ? null : "The product has not been inspected by the Inspector.",
                CreatedAt = bike.CreatedAt,
                ImageUrls = bike.BikeImages.Select(bi => bi.ImageUrl).ToList(),
                InspectionReports = reports
            };
        }

        public async Task<IEnumerable<BikeResponseDto>> GetAllAsync(int? categoryId = null, int? brandId = null, decimal? minPrice = null, decimal? maxPrice = null, string? searchTitle = null)
        {
            var query = _context.Bikes
                .Include(b => b.Seller)
                .Include(b => b.Brand)
                .Include(b => b.Category)
                .Include(b => b.BikeImages)
                .Include(b => b.InspectionReports)
                    .ThenInclude(ir => ir.Inspector)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(b => b.CategoryId == categoryId.Value);

            if (brandId.HasValue)
                query = query.Where(b => b.BrandId == brandId.Value);

            if (minPrice.HasValue)
                query = query.Where(b => b.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(b => b.Price <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(searchTitle))
            {
                var lowerSearch = searchTitle.ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(lowerSearch));
            }

            var bikes = await query.ToListAsync();
            return bikes.Select(MapBikeToResponse);
        }

        public async Task<BikeResponseDto?> GetByIdAsync(int id)
        {
            var bike = await _context.Bikes
                .Include(b => b.Seller)
                .Include(b => b.Brand)
                .Include(b => b.Category)
                .Include(b => b.BikeImages)
                .Include(b => b.InspectionReports)
                    .ThenInclude(ir => ir.Inspector)
                .FirstOrDefaultAsync(b => b.BikeId == id);

            if (bike == null) return null;

            return MapBikeToResponse(bike);
        }

        public async Task<BikeResponseDto> CreateAsync(CreateBikeDto dto)
        {
            // Validate that SellerId is a user with SELLER role (roleId=3)
            var seller = await _context.Users.FindAsync(dto.SellerId);
            if (seller == null || seller.RoleId != 3)
                throw new InvalidOperationException("Chỉ user có role SELLER mới được tạo listing.");

            BikeCondition? bikeCondition = null;
            if (!string.IsNullOrWhiteSpace(dto.BikeCondition))
            {
                if (!TryParseBikeCondition(dto.BikeCondition, out var parsedCondition))
                    throw new InvalidOperationException("BikeCondition không hợp lệ. Giá trị hợp lệ: NEW, USED_LIKE_NEW, USED_GOOD, USED_FAIR.");

                bikeCondition = parsedCondition;
            }

            var bike = new Bike
            {
                SellerId = dto.SellerId,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
                FrameSize = dto.FrameSize,
                BikeCondition = bikeCondition,
                Status = BikeStatus.PENDING, // Always PENDING on creation
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

            if (dto.BikeCondition != null && TryParseBikeCondition(dto.BikeCondition, out var condition))
                bike.BikeCondition = condition;

            if (dto.Status != null && Enum.TryParse<BikeStatus>(dto.Status, ignoreCase: true, out var status))
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

        public async Task<bool> ApproveAsync(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null || bike.Status != BikeStatus.PENDING) return false;

            bike.Status = BikeStatus.APPROVED;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null || bike.Status != BikeStatus.PENDING) return false;

            bike.Status = BikeStatus.REJECTED;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
