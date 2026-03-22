using Backend_CycleTrust.BLL.DTOs.UserDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<UserResponseDto> CreateAsync(CreateUserDto dto);
        Task<bool> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> UpdateRoleAsync(int userId, int newRoleId);
        Task<bool> RequestSellerRoleAsync(int userId);
        Task<bool> DeleteAsync(int id);
        Task<bool> BanAsync(int userId);
        Task<bool> UnbanAsync(int userId);
    }
}
