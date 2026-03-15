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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportResponseDto>>> GetAll()
        {
            var reports = await _reportService.GetAllAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportResponseDto>> GetById(int id)
        {
            var report = await _reportService.GetByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        /// <summary>
        /// Buyer tạo báo cáo tranh chấp.
        /// </summary>
        [Authorize(Roles = "BUYER")]
        [HttpPost]
        public async Task<ActionResult<ReportResponseDto>> Create(CreateReportDto dto)
        {
            var report = await _reportService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = report.ReportId }, report);
        }

        /// <summary>
        /// Admin xử lý tranh chấp (Resolved/Rejected).
        /// </summary>
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReportDto dto)
        {
            var result = await _reportService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reportService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
