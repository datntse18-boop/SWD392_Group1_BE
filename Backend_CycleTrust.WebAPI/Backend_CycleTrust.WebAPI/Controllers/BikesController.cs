using Backend_CycleTrust.BLL.DTOs.BikeDTOs;
using Backend_CycleTrust.BLL.DTOs.BikeImageDTOs;
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
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IBikeImageService _bikeImageService;
        private readonly ILogger<BikesController> _logger;

        public BikesController(
            IBikeService bikeService, 
            ICloudinaryService cloudinaryService, 
            IBikeImageService bikeImageService,
            ILogger<BikesController> logger)
        {
            _bikeService = bikeService;
            _cloudinaryService = cloudinaryService;
            _bikeImageService = bikeImageService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeResponseDto>>> GetAll(
            [FromQuery] int? categoryId,
            [FromQuery] int? brandId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? searchTitle)
        {
            var bikes = await _bikeService.GetAllAsync(categoryId, brandId, minPrice, maxPrice, searchTitle);
            return Ok(bikes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BikeResponseDto>> GetById(int id)
        {
            var bike = await _bikeService.GetByIdAsync(id);
            if (bike == null) return NotFound();
            return Ok(bike);
        }

        [HttpPost]
        public async Task<ActionResult<BikeResponseDto>> Create(CreateBikeDto dto)
        {
            var bike = await _bikeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = bike.BikeId }, bike);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBikeDto dto)
        {
            var result = await _bikeService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bikeService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/images")]
        public async Task<ActionResult<string>> UploadImage(int id, IFormFile file)
        {
            try
            {
                var bike = await _bikeService.GetByIdAsync(id);
                if (bike == null) return NotFound("Bike not found.");

                var imageUrl = await _cloudinaryService.UploadImageAsync(file, "bikes");
                
                // Save URL to database
                await _bikeImageService.CreateAsync(new CreateBikeImageDto
                {
                    BikeId = id,
                    ImageUrl = imageUrl
                });
                
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image for bike {BikeId}: {Message}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        // ===== Admin: Approve / Reject Listing (FR-13) =====
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _bikeService.ApproveAsync(id);
            if (!result) return BadRequest(new { message = "Bike not found or not in PENDING status." });
            return Ok(new { message = "Bike listing approved successfully." });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _bikeService.RejectAsync(id);
            if (!result) return BadRequest(new { message = "Bike not found or not in PENDING status." });
            return Ok(new { message = "Bike listing rejected successfully." });
        }
    }
}