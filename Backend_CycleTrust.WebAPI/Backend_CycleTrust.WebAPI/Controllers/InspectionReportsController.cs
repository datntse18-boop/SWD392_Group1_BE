using System.Security.Claims;
using Backend_CycleTrust.BLL.DTOs.InspectionReportDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InspectionReportsController : ControllerBase
    {
        private readonly IInspectionReportService _inspectionReportService;

        public InspectionReportsController(IInspectionReportService inspectionReportService)
        {
            _inspectionReportService = inspectionReportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InspectionReportResponseDto>>> GetAll()
        {
            var reports = await _inspectionReportService.GetAllAsync();
            return Ok(reports);
        }

        [Authorize(Roles = "INSPECTOR")]
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<InspectionReportResponseDto>>> GetPending()
        {
            var reports = await _inspectionReportService.GetPendingRequestsAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionReportResponseDto>> GetById(int id)
        {
            var report = await _inspectionReportService.GetByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        /// <summary>
        /// Inspector tạo báo cáo kiểm định mới.
        /// </summary>
        [Authorize(Roles = "INSPECTOR")]
        [HttpPost]
        public async Task<ActionResult<InspectionReportResponseDto>> Create(CreateInspectionReportDto dto)
        {
            var report = await _inspectionReportService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = report.ReportId }, report);
        }

        /// <summary>
        /// Inspector cập nhật báo cáo (Pass/Fail).
        /// </summary>
        [Authorize(Roles = "INSPECTOR")]
        [HttpPut("{id}/complete")]
        public async Task<ActionResult<InspectionReportResponseDto>> Complete(int id, CompleteInspectionReportDto dto)
        {
            var inspectorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(inspectorIdClaim) || !int.TryParse(inspectorIdClaim, out var inspectorId))
                return Unauthorized(new { message = "Invalid token: inspector ID not found." });

            try
            {
                var report = await _inspectionReportService.CompleteInspectionAsync(id, inspectorId, dto);
                return Ok(report);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "INSPECTOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateInspectionReportDto dto)
        {
            var result = await _inspectionReportService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _inspectionReportService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
