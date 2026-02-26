using Backend_CycleTrust.BLL.DTOs.ReportDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly CycleTrustDbContext _context;

        public ReportService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReportResponseDto>> GetAllAsync()
        {
            return await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Bike)
                .Select(r => new ReportResponseDto
                {
                    ReportId = r.ReportId,
                    ReporterId = r.ReporterId,
                    ReporterName = r.Reporter.FullName,
                    BikeId = r.BikeId,
                    BikeTitle = r.Bike != null ? r.Bike.Title : null,
                    Reason = r.Reason,
                    Status = r.Status.ToString(),
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ReportResponseDto?> GetByIdAsync(int id)
        {
            var report = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Bike)
                .FirstOrDefaultAsync(r => r.ReportId == id);

            if (report == null) return null;

            return new ReportResponseDto
            {
                ReportId = report.ReportId,
                ReporterId = report.ReporterId,
                ReporterName = report.Reporter.FullName,
                BikeId = report.BikeId,
                BikeTitle = report.Bike?.Title,
                Reason = report.Reason,
                Status = report.Status.ToString(),
                CreatedAt = report.CreatedAt
            };
        }

        public async Task<ReportResponseDto> CreateAsync(CreateReportDto dto)
        {
            var report = new Report
            {
                ReporterId = dto.ReporterId,
                BikeId = dto.BikeId,
                Reason = dto.Reason,
                Status = ReportStatus.PENDING,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(report.ReportId) ?? throw new Exception("Failed to create report");
        }

        public async Task<bool> UpdateAsync(int id, UpdateReportDto dto)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null) return false;

            if (Enum.TryParse<ReportStatus>(dto.Status, out var status))
                report.Status = status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null) return false;

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
