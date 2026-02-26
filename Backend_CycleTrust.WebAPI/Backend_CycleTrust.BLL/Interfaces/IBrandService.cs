using Backend_CycleTrust.BLL.DTOs.BrandDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponseDto>> GetAllAsync();
        Task<BrandResponseDto?> GetByIdAsync(int id);
        Task<BrandResponseDto> CreateAsync(CreateBrandDto dto);
        Task<bool> UpdateAsync(int id, UpdateBrandDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
