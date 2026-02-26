using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_CycleTrust.DAL.Entities
{
    [Table("messages")]
    public class Message
    {
        [Key]
        [Column("message_id")]
        public int MessageId { get; set; }

        [Column("sender_id")]
        public int SenderId { get; set; }

        [Column("receiver_id")]
        public int ReceiverId { get; set; }

        [Column("bike_id")]
        public int? BikeId { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; } = null!;

        [Column("sent_at")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; } = null!;

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; } = null!;

        [ForeignKey("BikeId")]
        public virtual Bike? Bike { get; set; }
    }
}
