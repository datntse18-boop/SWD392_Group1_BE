using Backend_CycleTrust.BLL.DTOs.WishlistDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistResponseDto>> GetAllAsync();
        Task<WishlistResponseDto?> GetByIdAsync(int id);
        Task<WishlistResponseDto> CreateAsync(CreateWishlistDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
