using Backend_CycleTrust.BLL.DTOs.ReviewDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetAll()
        {
            var reviews = await _reviewService.GetAllAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewResponseDto>> GetById(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewResponseDto>> Create(CreateReviewDto dto)
        {
            var review = await _reviewService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReviewDto dto)
        {
            var result = await _reviewService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
