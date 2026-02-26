using Backend_CycleTrust.BLL.DTOs.RoleDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Interfaces;

namespace Backend_CycleTrust.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _repository;

        public RoleService(IGenericRepository<Role> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
        {
            var roles = await _repository.GetAllAsync();
            return roles.Select(r => new RoleResponseDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName
            });
        }

        public async Task<RoleResponseDto?> GetByIdAsync(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null) return null;
            return new RoleResponseDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }

        public async Task<RoleResponseDto> CreateAsync(CreateRoleDto dto)
        {
            var role = new Role { RoleName = dto.RoleName };
            await _repository.AddAsync(role);
            return new RoleResponseDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateRoleDto dto)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null) return false;

            role.RoleName = dto.RoleName;
            await _repository.UpdateAsync(role);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null) return false;

            await _repository.DeleteAsync(role);
            return true;
        }
    }
}
