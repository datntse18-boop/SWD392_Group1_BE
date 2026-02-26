using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend_CycleTrust.DAL.Enums;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("reports")]
    public class Report
    {
        [Key]
        [Column("report_id")]
        public int ReportId { get; set; }

        [Column("reporter_id")]
        public int ReporterId { get; set; }

        [Column("bike_id")]
        public int? BikeId { get; set; }

        [Required]
        [Column("reason")]
        public string Reason { get; set; } = null!;

        [Column("status")]
        public ReportStatus Status { get; set; } = ReportStatus.PENDING;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("ReporterId")]
        public virtual User Reporter { get; set; } = null!;

        [ForeignKey("BikeId")]
        public virtual Bike? Bike { get; set; }
    }
}
