namespace Backend_CycleTrust.BLL.DTOs.BikeDTOs
{
    public class CreateBikeDto
    {
        public int SellerId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string? FrameSize { get; set; }
        public string? BikeCondition { get; set; }
    }

    public class UpdateBikeDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string? FrameSize { get; set; }
        public string? BikeCondition { get; set; }
        public string? Status { get; set; }
    }

    public class BikeResponseDto
    {
        public int BikeId { get; set; }
        public int SellerId { get; set; }
        public string? SellerName { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? FrameSize { get; set; }
        public string? BikeCondition { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }
}
