using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("wishlist")]
    public class Wishlist
    {
        [Key]
        [Column("wishlist_id")]
        public int WishlistId { get; set; }

        [Column("buyer_id")]
        public int BuyerId { get; set; }

        [Column("bike_id")]
        public int BikeId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("BuyerId")]
        public virtual User Buyer { get; set; } = null!;

        [ForeignKey("BikeId")]
        public virtual Bike Bike { get; set; } = null!;
    }
}
