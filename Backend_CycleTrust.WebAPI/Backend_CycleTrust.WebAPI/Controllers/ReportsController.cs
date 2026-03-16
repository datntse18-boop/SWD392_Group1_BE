using System.Security.Claims;
using Backend_CycleTrust.BLL.DTOs.ReportDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportResponseDto>>> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [Authorize(Roles = "BUYER")]
        [HttpGet("my-reports")]
        public async Task<ActionResult<IEnumerable<ReportResponseDto>>> GetMyReports()
        {
            var reporterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(reporterIdClaim) || !int.TryParse(reporterIdClaim, out var reporterId))
                return Unauthorized(new { message = "Invalid token: reporter ID not found." });

            var reports = await _reportService.GetReportsByReporterAsync(reporterId);
            return Ok(reports);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportResponseDto>> GetById(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [Authorize(Roles = "BUYER")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ReportResponseDto>> Create([FromForm] CreateReportDto dto)
        {
            var reporterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(reporterIdClaim) || !int.TryParse(reporterIdClaim, out var reporterId))
                return Unauthorized(new { message = "Invalid token: reporter ID not found." });

            dto.ReporterId = reporterId;

            try
            {
                var report = await _reportService.CreateReportAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = report.ReportId }, report);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "BUYER")]
        [HttpPut("{id}/my-report")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateMyPendingReport(int id, [FromForm] UpdateOwnReportDto dto)
        {
            var reporterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(reporterIdClaim) || !int.TryParse(reporterIdClaim, out var reporterId))
                return Unauthorized(new { message = "Invalid token: reporter ID not found." });

            try
            {
                var updated = await _reportService.UpdateOwnPendingReportAsync(id, reporterId, dto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "BUYER")]
        [HttpDelete("{id}/my-report")]
        public async Task<IActionResult> DeleteMyPendingReport(int id)
        {
            var reporterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(reporterIdClaim) || !int.TryParse(reporterIdClaim, out var reporterId))
                return Unauthorized(new { message = "Invalid token: reporter ID not found." });

            try
            {
                var deleted = await _reportService.DeleteOwnPendingReportAsync(id, reporterId);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReportDto dto)
        {
            try
            {
                var updated = await _reportService.UpdateReportStatusAsync(id, dto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
