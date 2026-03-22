using Backend_CycleTrust.BLL.DTOs.UserDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Enums;
using Backend_CycleTrust.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly CycleTrustDbContext _context;

        public UserService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserResponseDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    RoleId = u.RoleId,
                    RoleName = u.Role.RoleName,
                    Status = u.Status.ToString(),
                    CreatedAt = u.CreatedAt,
                    PendingSellerUpgrade = u.PendingSellerUpgrade
                })
                .ToListAsync();
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return null;

            return new UserResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                Status = user.Status.ToString(),
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                Address = dto.Address,
                RoleId = dto.RoleId,
                Status = UserStatus.ACTIVE,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _context.Entry(user).Reference(u => u.Role).LoadAsync();

            return new UserResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                Status = user.Status.ToString(),
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Address = dto.Address;
            user.RoleId = dto.RoleId;
            if (Enum.TryParse<UserStatus>(dto.Status, out var status))
                user.Status = status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoleAsync(int userId, int newRoleId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == newRoleId);
            if (!roleExists) return false;

            user.RoleId = newRoleId;
            user.PendingSellerUpgrade = false; // Reset if approved/changed
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RequestSellerRoleAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Only allow standard buyers to request
            if (user.RoleId != 2) return false;

            user.PendingSellerUpgrade = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Status == UserStatus.BANNED) return false;

            user.Status = UserStatus.BANNED;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnbanAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Status == UserStatus.ACTIVE) return false;

            user.Status = UserStatus.ACTIVE;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
