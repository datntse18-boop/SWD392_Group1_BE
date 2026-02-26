using Backend_CycleTrust.BLL.DTOs.AuthDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
        Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto);
    }
}
