using Backend_CycleTrust.BLL.DTOs.BikeDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IBikeService
    {
        Task<IEnumerable<BikeResponseDto>> GetAllAsync();
        Task<BikeResponseDto?> GetByIdAsync(int id);
        Task<BikeResponseDto> CreateAsync(CreateBikeDto dto);
        Task<bool> UpdateAsync(int id, UpdateBikeDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ApproveAsync(int bikeId);
        Task<bool> RejectAsync(int bikeId);
    }
}
