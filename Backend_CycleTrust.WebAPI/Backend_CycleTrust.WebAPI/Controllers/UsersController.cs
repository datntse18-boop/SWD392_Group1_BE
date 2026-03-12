using Backend_CycleTrust.BLL.DTOs.UserDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(CreateUserDto dto)
        {
            var user = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var result = await _userService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // ===== Admin: Ban / Unban User (FR-14) =====

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}/ban")]
        public async Task<IActionResult> Ban(int id)
        {
            var result = await _userService.BanAsync(id);
            if (!result) return BadRequest(new { message = "User not found or already banned." });
            return Ok(new { message = "User has been banned successfully." });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}/unban")]
        public async Task<IActionResult> Unban(int id)
        {
            var result = await _userService.UnbanAsync(id);
            if (!result) return BadRequest(new { message = "User not found or already active." });
            return Ok(new { message = "User has been unbanned successfully." });
        }
    }
}
