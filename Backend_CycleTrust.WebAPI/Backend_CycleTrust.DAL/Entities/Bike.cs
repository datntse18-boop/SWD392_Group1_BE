using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend_CycleTrust.DAL.Enums;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("bikes")]
    public class Bike
    {
        [Key]
        [Column("bike_id")]
        public int BikeId { get; set; }

        [Column("seller_id")]
        public int SellerId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("price", TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        [Column("brand_id")]
        public int? BrandId { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }

        [MaxLength(50)]
        [Column("frame_size")]
        public string? FrameSize { get; set; }

        [Column("bike_condition")]
        public BikeCondition? BikeCondition { get; set; }

        [Column("status")]
        public BikeStatus Status { get; set; } = BikeStatus.PENDING;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; } = null!;

        [ForeignKey("BrandId")]
        public virtual Brand? Brand { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<BikeImage> BikeImages { get; set; } = new List<BikeImage>();
        public virtual ICollection<InspectionReport> InspectionReports { get; set; } = new List<InspectionReport>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    }
}
