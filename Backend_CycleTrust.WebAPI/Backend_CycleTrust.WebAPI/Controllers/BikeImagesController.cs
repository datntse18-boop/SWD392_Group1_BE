using Backend_CycleTrust.BLL.DTOs.BikeImageDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeImagesController : ControllerBase
    {
        private readonly IBikeImageService _bikeImageService;

        public BikeImagesController(IBikeImageService bikeImageService)
        {
            _bikeImageService = bikeImageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeImageResponseDto>>> GetAll()
        {
            var images = await _bikeImageService.GetAllAsync();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BikeImageResponseDto>> GetById(int id)
        {
            var image = await _bikeImageService.GetByIdAsync(id);
            if (image == null) return NotFound();
            return Ok(image);
        }

        [HttpPost]
        public async Task<ActionResult<BikeImageResponseDto>> Create(CreateBikeImageDto dto)
        {
            var image = await _bikeImageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = image.ImageId }, image);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bikeImageService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
