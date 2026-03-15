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

        /// <summary>
        /// Admin chuyển role cho user (ví dụ: Buyer → Seller).
        /// </summary>
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateUserRoleDto dto)
        {
            var result = await _userService.UpdateRoleAsync(id, dto.RoleId);
            if (!result) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Buyer yêu cầu trở thành Seller.
        /// </summary>
        [HttpPost("{id}/request-seller")]
        [Authorize(Roles = "BUYER")]
        public async Task<IActionResult> RequestSellerRole(int id)
        {
            var result = await _userService.RequestSellerRoleAsync(id);
            if (!result) return BadRequest("Could not request seller role. Ensure user exists and is a Buyer.");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
