using Backend_CycleTrust.BLL.DTOs.WishlistDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistsController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistResponseDto>>> GetAll()
        {
            var wishlists = await _wishlistService.GetAllAsync();
            return Ok(wishlists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WishlistResponseDto>> GetById(int id)
        {
            var wishlist = await _wishlistService.GetByIdAsync(id);
            if (wishlist == null) return NotFound();
            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<ActionResult<WishlistResponseDto>> Create(CreateWishlistDto dto)
        {
            var wishlist = await _wishlistService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = wishlist.WishlistId }, wishlist);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _wishlistService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
