using Backend_CycleTrust.BLL.DTOs.BikeDTOs;
using Backend_CycleTrust.BLL.Interfaces;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeResponseDto>>> GetAll()
        {
            var bikes = await _bikeService.GetAllAsync();
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
    }
}
