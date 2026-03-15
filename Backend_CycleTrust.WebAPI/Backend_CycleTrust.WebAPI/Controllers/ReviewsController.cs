using Backend_CycleTrust.BLL.DTOs.ReviewDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        /// <summary>
        /// Buyer tạo đánh giá cho Seller sau giao dịch.
        /// </summary>
        [Authorize(Roles = "BUYER")]
        [HttpPost]
        public async Task<ActionResult<ReviewResponseDto>> Create(CreateReviewDto dto)
        {
            try
            {
                var review = await _reviewService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "BUYER,ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReviewDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var actorUserId))
                return Unauthorized(new { message = "Invalid token: user ID not found." });

            var isAdmin = User.IsInRole("ADMIN");

            try
            {
                var result = await _reviewService.UpdateAsync(id, dto, actorUserId, isAdmin);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "BUYER,ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var actorUserId))
                return Unauthorized(new { message = "Invalid token: user ID not found." });

            var isAdmin = User.IsInRole("ADMIN");

            try
            {
                var result = await _reviewService.DeleteAsync(id, actorUserId, isAdmin);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
