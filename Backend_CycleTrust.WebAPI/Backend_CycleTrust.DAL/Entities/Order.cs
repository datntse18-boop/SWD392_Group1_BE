using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend_CycleTrust.DAL.Enums;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("bike_id")]
        public int BikeId { get; set; }

        [Column("buyer_id")]
        public int BuyerId { get; set; }

        [Column("seller_id")]
        public int SellerId { get; set; }

        [Column("total_amount", TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; }

        [Column("deposit_amount", TypeName = "decimal(12,2)")]
        public decimal? DepositAmount { get; set; }

        [Column("status")]
        public OrderStatus Status { get; set; } = OrderStatus.PENDING;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("BikeId")]
        public virtual Bike Bike { get; set; } = null!;

        [ForeignKey("BuyerId")]
        public virtual User Buyer { get; set; } = null!;

        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; } = null!;

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
