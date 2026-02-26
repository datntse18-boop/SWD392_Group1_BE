namespace Backend_CycleTrust.BLL.DTOs.WishlistDTOs
{
    public class CreateWishlistDto
    {
        public int BuyerId { get; set; }
        public int BikeId { get; set; }
    }

    public class WishlistResponseDto
    {
        public int WishlistId { get; set; }
        public int BuyerId { get; set; }
        public string? BuyerName { get; set; }
        public int BikeId { get; set; }
        public string? BikeTitle { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
