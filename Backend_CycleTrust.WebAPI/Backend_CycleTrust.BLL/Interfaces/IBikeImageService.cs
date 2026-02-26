using Backend_CycleTrust.BLL.DTOs.BikeImageDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IBikeImageService
    {
        Task<IEnumerable<BikeImageResponseDto>> GetAllAsync();
        Task<BikeImageResponseDto?> GetByIdAsync(int id);
        Task<BikeImageResponseDto> CreateAsync(CreateBikeImageDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
