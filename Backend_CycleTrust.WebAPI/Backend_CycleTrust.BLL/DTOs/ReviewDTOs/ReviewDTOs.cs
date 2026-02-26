namespace Backend_CycleTrust.BLL.DTOs.ReviewDTOs
{
    public class CreateReviewDto
    {
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }

    public class UpdateReviewDto
    {
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }

    public class ReviewResponseDto
    {
        public int ReviewId { get; set; }
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public string? BuyerName { get; set; }
        public int SellerId { get; set; }
        public string? SellerName { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
