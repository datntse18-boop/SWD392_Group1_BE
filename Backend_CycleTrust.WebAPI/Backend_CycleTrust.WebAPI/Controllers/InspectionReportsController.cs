using Backend_CycleTrust.BLL.DTOs.InspectionReportDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionReportResponseDto>> GetById(int id)
        {
            var report = await _inspectionReportService.GetByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [HttpPost]
        public async Task<ActionResult<InspectionReportResponseDto>> Create(CreateInspectionReportDto dto)
        {
            var report = await _inspectionReportService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = report.ReportId }, report);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateInspectionReportDto dto)
        {
            var result = await _inspectionReportService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _inspectionReportService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
