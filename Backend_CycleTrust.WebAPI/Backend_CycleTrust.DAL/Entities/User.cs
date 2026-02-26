using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend_CycleTrust.DAL.Enums;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("full_name")]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        [Column("password")]
        public string Password { get; set; } = null!;

        [MaxLength(20)]
        [Column("phone")]
        public string? Phone { get; set; }

        [MaxLength(255)]
        [Column("address")]
        public string? Address { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("status")]
        public UserStatus Status { get; set; } = UserStatus.ACTIVE;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;

        public virtual ICollection<Bike> Bikes { get; set; } = new List<Bike>();
        public virtual ICollection<Order> BuyerOrders { get; set; } = new List<Order>();
        public virtual ICollection<Order> SellerOrders { get; set; } = new List<Order>();
        public virtual ICollection<Review> BuyerReviews { get; set; } = new List<Review>();
        public virtual ICollection<Review> SellerReviews { get; set; } = new List<Review>();
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
        public virtual ICollection<InspectionReport> InspectionReports { get; set; } = new List<InspectionReport>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    }
}
