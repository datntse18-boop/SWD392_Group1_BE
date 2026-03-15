using Backend_CycleTrust.BLL.DTOs.BikeDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly IBikeService _bikeService;

        public BikesController(IBikeService bikeService)
        {
            _bikeService = bikeService;
        }

        /// <summary>
        /// Lấy danh sách tất cả xe (public).
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeResponseDto>>> GetAll()
        {
            var bikes = await _bikeService.GetAllAsync();
            return Ok(bikes);
        }

        /// <summary>
        /// Lấy xe theo ID (public).
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<BikeResponseDto>> GetById(int id)
        {
            var bike = await _bikeService.GetByIdAsync(id);
            if (bike == null) return NotFound();
            return Ok(bike);
        }

        /// <summary>
        /// Seller tạo listing mới (status mặc định = PENDING).
        /// </summary>
        [Authorize(Roles = "SELLER")]
        [HttpPost]
        public async Task<ActionResult<BikeResponseDto>> Create(CreateBikeDto dto)
        {
            var bike = await _bikeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = bike.BikeId }, bike);
        }

        /// <summary>
        /// Admin duyệt/từ chối listing hoặc Seller chỉnh sửa listing.
        /// </summary>
        [Authorize(Roles = "ADMIN,SELLER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBikeDto dto)
        {
            var result = await _bikeService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bikeService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
