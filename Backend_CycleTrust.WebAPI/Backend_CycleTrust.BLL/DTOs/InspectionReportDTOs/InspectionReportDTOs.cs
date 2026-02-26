namespace Backend_CycleTrust.BLL.DTOs.InspectionReportDTOs
{
    public class CreateInspectionReportDto
    {
        public int BikeId { get; set; }
        public int InspectorId { get; set; }
        public string? FrameCondition { get; set; }
        public string? BrakeCondition { get; set; }
        public string? DrivetrainCondition { get; set; }
        public string? OverallComment { get; set; }
        public string? ReportFile { get; set; }
    }

    public class UpdateInspectionReportDto
    {
        public string? FrameCondition { get; set; }
        public string? BrakeCondition { get; set; }
        public string? DrivetrainCondition { get; set; }
        public string? OverallComment { get; set; }
        public string? ReportFile { get; set; }
        public string? InspectionStatus { get; set; }
    }

    public class InspectionReportResponseDto
    {
        public int ReportId { get; set; }
        public int BikeId { get; set; }
        public string? BikeTitle { get; set; }
        public int InspectorId { get; set; }
        public string? InspectorName { get; set; }
        public string? FrameCondition { get; set; }
        public string? BrakeCondition { get; set; }
        public string? DrivetrainCondition { get; set; }
        public string? OverallComment { get; set; }
        public string? ReportFile { get; set; }
        public string InspectionStatus { get; set; } = null!;
        public DateTime InspectedAt { get; set; }
    }
}
