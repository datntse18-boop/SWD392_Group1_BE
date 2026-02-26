namespace Backend_CycleTrust.BLL.DTOs.CategoryDTOs
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; } = null!;
    }

    public class UpdateCategoryDto
    {
        public string CategoryName { get; set; } = null!;
    }

    public class CategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
