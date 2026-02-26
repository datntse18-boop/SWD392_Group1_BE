using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend_CycleTrust.DAL.Enums;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("inspection_reports")]
    public class InspectionReport
    {
        [Key]
        [Column("report_id")]
        public int ReportId { get; set; }

        [Column("bike_id")]
        public int BikeId { get; set; }

        [Column("inspector_id")]
        public int InspectorId { get; set; }

        [Column("frame_condition")]
        public string? FrameCondition { get; set; }

        [Column("brake_condition")]
        public string? BrakeCondition { get; set; }

        [Column("drivetrain_condition")]
        public string? DrivetrainCondition { get; set; }

        [Column("overall_comment")]
        public string? OverallComment { get; set; }

        [MaxLength(500)]
        [Column("report_file")]
        public string? ReportFile { get; set; }

        [Column("inspection_status")]
        public InspectionStatus InspectionStatus { get; set; } = InspectionStatus.PENDING;

        [Column("inspected_at")]
        public DateTime InspectedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("BikeId")]
        public virtual Bike Bike { get; set; } = null!;

        [ForeignKey("InspectorId")]
        public virtual User Inspector { get; set; } = null!;
    }
}
