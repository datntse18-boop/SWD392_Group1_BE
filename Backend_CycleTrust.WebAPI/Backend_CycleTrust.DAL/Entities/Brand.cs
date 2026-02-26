using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("brands")]
    public class Brand
    {
        [Key]
        [Column("brand_id")]
        public int BrandId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("brand_name")]
        public string BrandName { get; set; } = null!;

        // Navigation
        public virtual ICollection<Bike> Bikes { get; set; } = new List<Bike>();
    }
}
