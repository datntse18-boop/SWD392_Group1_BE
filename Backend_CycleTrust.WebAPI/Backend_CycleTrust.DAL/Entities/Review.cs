using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("reviews")]
    public class Review
    {
        [Key]
        [Column("review_id")]
        public int ReviewId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("buyer_id")]
        public int BuyerId { get; set; }

        [Column("seller_id")]
        public int SellerId { get; set; }

        [Column("rating")]
        public int? Rating { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        [ForeignKey("BuyerId")]
        public virtual User Buyer { get; set; } = null!;

        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; } = null!;
    }
}
