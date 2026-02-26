using Backend_CycleTrust.BLL.DTOs.BrandDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandResponseDto>> GetById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<BrandResponseDto>> Create(CreateBrandDto dto)
        {
            var brand = await _brandService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = brand.BrandId }, brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBrandDto dto)
        {
            var result = await _brandService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _brandService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
