using Backend_CycleTrust.BLL.DTOs.BikeDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IBikeService
    {
        Task<IEnumerable<BikeResponseDto>> GetAllAsync(int? categoryId = null, int? brandId = null, decimal? minPrice = null, decimal? maxPrice = null, string? searchTitle = null);
        Task<BikeResponseDto?> GetByIdAsync(int id);
        Task<BikeResponseDto> CreateAsync(CreateBikeDto dto);
        Task<bool> UpdateAsync(int id, UpdateBikeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
