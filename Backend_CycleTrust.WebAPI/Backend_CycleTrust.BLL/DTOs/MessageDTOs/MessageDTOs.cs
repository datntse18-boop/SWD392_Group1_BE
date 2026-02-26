namespace Backend_CycleTrust.BLL.DTOs.MessageDTOs
{
    public class CreateMessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int? BikeId { get; set; }
        public string Content { get; set; } = null!;
    }

    public class MessageResponseDto
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public int? BikeId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime SentAt { get; set; }
    }
}
