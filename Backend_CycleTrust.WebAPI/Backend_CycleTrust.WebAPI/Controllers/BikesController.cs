using Backend_CycleTrust.BLL.DTOs.BikeDTOs;
using Backend_CycleTrust.BLL.DTOs.BikeImageDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly IBikeService _bikeService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IBikeImageService _bikeImageService;
        private readonly IInspectionReportService _inspectionReportService;
        private readonly ILogger<BikesController> _logger;

        public BikesController(
            IBikeService bikeService,
            ICloudinaryService cloudinaryService,
            IBikeImageService bikeImageService,
            IInspectionReportService inspectionReportService,
            ILogger<BikesController> logger)
        {
            _bikeService = bikeService;
            _cloudinaryService = cloudinaryService;
            _bikeImageService = bikeImageService;
            _inspectionReportService = inspectionReportService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả xe (public).
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeResponseDto>>> GetAll(
            [FromQuery] int? categoryId,
            [FromQuery] int? brandId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? searchTitle)
        {
            var bikes = await _bikeService.GetAllAsync(categoryId, brandId, minPrice, maxPrice, searchTitle);

            var canViewAll = User.IsInRole("ADMIN") || User.IsInRole("SELLER") || User.IsInRole("INSPECTOR");
            if (!canViewAll)
                bikes = bikes.Where(b => b.Status == "APPROVED");

            return Ok(bikes);
        }

        /// <summary>
        /// Lấy danh sách condition options (public).
        /// </summary>
        [AllowAnonymous]
        [HttpGet("conditions")]
        public IActionResult GetConditions()
        {
            var conditions = new[]
            {
                new { value = "NEW", label = "New" },
                new { value = "USED_LIKE_NEW", label = "Like New" },
                new { value = "USED_GOOD", label = "Used" },
                new { value = "USED_FAIR", label = "Needs Repair" }
            };

            return Ok(conditions);
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

            var canViewAll = User.IsInRole("ADMIN") || User.IsInRole("SELLER") || User.IsInRole("INSPECTOR");
            if (!canViewAll && bike.Status != "APPROVED")
                return NotFound();

            return Ok(bike);
        }

        /// <summary>
        /// Seller tạo listing mới (status mặc định = PENDING).
        /// </summary>
        [Authorize(Roles = "SELLER")]
        [HttpPost]
        public async Task<ActionResult<BikeResponseDto>> Create([FromBody] CreateBikeDto dto)
        {
            // ✅ FIX 2: Inject SellerId from JWT claims so service doesn't throw
            var sellerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(sellerIdClaim) || !int.TryParse(sellerIdClaim, out var sellerId))
                return Unauthorized(new { message = "Invalid token: seller ID not found." });

            dto.SellerId = sellerId;

            try
            {
                var bike = await _bikeService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = bike.BikeId }, bike);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bike listing for seller {SellerId}", sellerId);
                return BadRequest(new { message = ex.Message });
            }
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

        // ✅ FIX 3: Image endpoints should require auth (SELLER or ADMIN)
        [Authorize(Roles = "SELLER,ADMIN")]
        [HttpPost("{id}/images")]
        public async Task<ActionResult<string>> UploadImage(int id, IFormFile file)
        {
            try
            {
                var bike = await _bikeService.GetByIdAsync(id);
                if (bike == null) return NotFound("Bike not found.");

                var imageUrl = await _cloudinaryService.UploadImageAsync(file, "bikes");

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

        [Authorize(Roles = "SELLER,ADMIN")]
        [HttpDelete("{id}/images")]
        public async Task<IActionResult> DeleteImage(int id, [FromQuery] string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return BadRequest(new { message = "Image URL is required." });

            try
            {
                var bike = await _bikeService.GetByIdAsync(id);
                if (bike == null) return NotFound(new { message = "Bike not found." });

                var cloudinaryDeleted = await _cloudinaryService.DeleteImageAsync(imageUrl);
                if (!cloudinaryDeleted)
                    _logger.LogWarning("Failed to delete image from Cloudinary. URL: {Url}", imageUrl);

                var dbDeleted = await _bikeImageService.DeleteByUrlAsync(id, imageUrl);
                if (!dbDeleted)
                    return NotFound(new { message = "Image not found in database for this bike." });

                return Ok(new { message = "Image deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image for bike {BikeId}, URL {Url}: {Message}", id, imageUrl, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

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

        [Authorize(Roles = "SELLER")]
        [HttpPost("{id}/inspection-request")]
        public async Task<IActionResult> RequestInspection(int id)
        {
            var sellerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(sellerIdClaim) || !int.TryParse(sellerIdClaim, out var sellerId))
                return Unauthorized(new { message = "Invalid token: seller ID not found." });

            try
            {
                var request = await _inspectionReportService.RequestInspectionAsync(id, sellerId);
                return Ok(new
                {
                    message = "Inspection request submitted successfully.",
                    request
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting inspection for bike {BikeId} by seller {SellerId}", id, sellerId);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}