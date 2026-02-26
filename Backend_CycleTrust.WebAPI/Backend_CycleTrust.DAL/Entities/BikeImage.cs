using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("bike_images")]
    public class BikeImage
    {
        [Key]
        [Column("image_id")]
        public int ImageId { get; set; }

        [Column("bike_id")]
        public int BikeId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("image_url")]
        public string ImageUrl { get; set; } = null!;

        // Navigation
        [ForeignKey("BikeId")]
        public virtual Bike Bike { get; set; } = null!;
    }
}
