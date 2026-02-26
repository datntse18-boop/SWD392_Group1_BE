using Backend_CycleTrust.BLL.DTOs.RoleDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponseDto>> GetAllAsync();
        Task<RoleResponseDto?> GetByIdAsync(int id);
        Task<RoleResponseDto> CreateAsync(CreateRoleDto dto);
        Task<bool> UpdateAsync(int id, UpdateRoleDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
