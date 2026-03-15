namespace Backend_CycleTrust.BLL.DTOs.MessageDTOs
{
    public class CreateMessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int? BikeId { get; set; }
        public string Content { get; set; } = null!;
    }

    public class InboxMessageDto
    {
        public int OtherUserId { get; set; }
        public string? OtherUserName { get; set; }
        public int? BikeId { get; set; }
        public string? BikeTitle { get; set; }
        public int LastMessageId { get; set; }
        public int LastSenderId { get; set; }
        public string LastMessageContent { get; set; } = null!;
        public DateTime LastSentAt { get; set; }
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
