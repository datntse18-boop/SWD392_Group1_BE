namespace Backend_CycleTrust.BLL.DTOs.BikeImageDTOs
{
    public class CreateBikeImageDto
    {
        public int BikeId { get; set; }
        public string ImageUrl { get; set; } = null!;
    }

    public class BikeImageResponseDto
    {
        public int ImageId { get; set; }
        public int BikeId { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
