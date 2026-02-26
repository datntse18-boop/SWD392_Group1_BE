namespace Backend_CycleTrust.BLL.DTOs.BrandDTOs
{
    public class CreateBrandDto
    {
        public string BrandName { get; set; } = null!;
    }

    public class UpdateBrandDto
    {
        public string BrandName { get; set; } = null!;
    }

    public class BrandResponseDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;
    }
}
