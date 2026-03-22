using Backend_CycleTrust.BLL.DTOs.BikeDTOs;
using Backend_CycleTrust.BLL.DTOs.ChatbotDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly CycleTrustDbContext _context;

        public ChatbotService(CycleTrustDbContext context)
        {
            _context = context;
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
                SellerName = bike.Seller?.FullName,
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

        public async Task<ChatbotSuggestResponseDto> SuggestBikesAsync(ChatbotSuggestRequestDto request)
        {
            var query = _context.Bikes
                .Include(b => b.Seller)
                .Include(b => b.Brand)
                .Include(b => b.Category)
                .Include(b => b.BikeImages)
                .Include(b => b.InspectionReports)
                    .ThenInclude(ir => ir.Inspector)
                .Where(b => b.Status == BikeStatus.APPROVED) // Only suggest approved bikes
                .AsQueryable();

            string message = "Dựa trên thông tin của bạn, CycleTrust xin gợi ý một số chiếc xe sau nha: \n";

            if (request.Weight.HasValue && request.Weight.Value > 90)
            {
                message += $"- Với cân nặng {request.Weight.Value}kg, bạn nên ưu tiên các dòng xe khung nhôm, khung sắt chắc chắn và phuộc giảm xóc tốt để đảm bảo an toàn & độ bền của xe.\n";
            }

            if (request.Height.HasValue)
            {
                string recSize = "";
                if (request.Height.Value < 160) recSize = "S";
                else if (request.Height.Value <= 175) recSize = "M";
                else recSize = "L";

                message += $"- Chiều cao {request.Height.Value}cm phù hợp nhất với xe khung size {recSize}.\n";
            }

            if (!string.IsNullOrWhiteSpace(request.BikeType))
            {
                var lowerType = request.BikeType.ToLowerInvariant();
                query = query.Where(b => b.Category != null && b.Category.CategoryName.ToLower().Contains(lowerType));
                message += $"- Loại xe bạn tìm: {request.BikeType}.\n";
            }

            if (request.MaxBudget.HasValue)
            {
                query = query.Where(b => b.Price <= request.MaxBudget.Value);
                message += $"- Ngân sách tối đa: {request.MaxBudget.Value:N0} VNĐ.\n";
            }

            if (request.MinBudget.HasValue)
            {
                query = query.Where(b => b.Price >= request.MinBudget.Value);
            }

            var bikes = await query.OrderByDescending(b => b.CreatedAt).Take(5).ToListAsync();
            var suggestedBikes = bikes.Select(MapBikeToResponse).ToList();

            if (!suggestedBikes.Any())
            {
                message = "Tiếc quá, hiện tại CycleTrust chưa có mẫu xe nào hoàn toàn phù hợp với các tiêu chí trên. Bạn có thể thử thay đổi mức giá hoặc loại hình xe nhé!";
            }
            else
            {
                message += $"\nTUYỆT VỜI! CycleTrust gợi ý cho bạn {suggestedBikes.Count} mẫu xe tốt nhất dưới đây:";
            }

            return new ChatbotSuggestResponseDto
            {
                Message = message,
                SuggestedBikes = suggestedBikes
            };
        }
    }
}
