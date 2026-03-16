using Microsoft.AspNetCore.Http;

namespace Backend_CycleTrust.BLL.DTOs.ReportDTOs
{
    public class CreateReportDto
    {
        public int ReporterId { get; set; }
        public int? BikeId { get; set; }
        public string Reason { get; set; } = null!;
        public List<IFormFile>? EvidenceImages { get; set; }
        public List<string>? ImageUrls { get; set; }
    }

    public class UpdateReportDto
    {
        public string Status { get; set; } = null!;
    }

    public class UpdateOwnReportDto
    {
        public string Reason { get; set; } = null!;
        public List<IFormFile>? EvidenceImages { get; set; }
        public List<string>? ImageUrls { get; set; }
    }

    public class ReportResponseDto
    {
        public int ReportId { get; set; }
        public int ReporterId { get; set; }
        public string? ReporterName { get; set; }
        public int? BikeId { get; set; }
        public string? BikeTitle { get; set; }
        public int? SellerId { get; set; }
        public string? SellerName { get; set; }
        public string Reason { get; set; } = null!;
        public List<string>? ImageUrls { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
