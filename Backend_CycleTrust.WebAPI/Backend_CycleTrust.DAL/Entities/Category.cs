using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("category_name")]
        public string CategoryName { get; set; } = null!;

        // Navigation
        public virtual ICollection<Bike> Bikes { get; set; } = new List<Bike>();
    }
}
