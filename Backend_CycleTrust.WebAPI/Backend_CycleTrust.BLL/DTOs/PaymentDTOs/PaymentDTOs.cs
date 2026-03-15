namespace Backend_CycleTrust.BLL.DTOs.PaymentDTOs
{
    public class PaymentSuccessRequestDto
    {
        public int OrderId { get; set; }
        public string? PaymentMethod { get; set; }
    }

    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}