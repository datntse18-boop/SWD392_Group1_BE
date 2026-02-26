namespace Backend_CycleTrust.BLL.DTOs.OrderDTOs
{
    public class CreateOrderDto
    {
        public int BikeId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DepositAmount { get; set; }
    }

    public class UpdateOrderDto
    {
        public decimal? DepositAmount { get; set; }
        public string Status { get; set; } = null!;
    }

    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public int BikeId { get; set; }
        public string? BikeTitle { get; set; }
        public int BuyerId { get; set; }
        public string? BuyerName { get; set; }
        public int SellerId { get; set; }
        public string? SellerName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DepositAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
