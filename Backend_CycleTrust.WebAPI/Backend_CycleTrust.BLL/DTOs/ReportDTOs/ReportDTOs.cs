namespace Backend_CycleTrust.BLL.DTOs.ReportDTOs
{
    public class CreateReportDto
    {
        public int ReporterId { get; set; }
        public int? BikeId { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class UpdateReportDto
    {
        public string Status { get; set; } = null!;
    }

    public class ReportResponseDto
    {
        public int ReportId { get; set; }
        public int ReporterId { get; set; }
        public string? ReporterName { get; set; }
        public int? BikeId { get; set; }
        public string? BikeTitle { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
