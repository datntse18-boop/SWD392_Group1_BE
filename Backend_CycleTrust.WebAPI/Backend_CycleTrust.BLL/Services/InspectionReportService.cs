using Backend_CycleTrust.BLL.DTOs.InspectionReportDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class InspectionReportService : IInspectionReportService
    {
        private readonly CycleTrustDbContext _context;

        public InspectionReportService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InspectionReportResponseDto>> GetAllAsync()
        {
            return await _context.InspectionReports
                .Include(ir => ir.Bike)
                .Include(ir => ir.Inspector)
                .Select(ir => new InspectionReportResponseDto
                {
                    ReportId = ir.ReportId,
                    BikeId = ir.BikeId,
                    BikeTitle = ir.Bike.Title,
                    InspectorId = ir.InspectorId,
                    InspectorName = ir.Inspector.FullName,
                    FrameCondition = ir.FrameCondition,
                    BrakeCondition = ir.BrakeCondition,
                    DrivetrainCondition = ir.DrivetrainCondition,
                    OverallComment = ir.OverallComment,
                    ReportFile = ir.ReportFile,
                    InspectionStatus = ir.InspectionStatus.ToString(),
                    InspectedAt = ir.InspectedAt
                })
                .ToListAsync();
        }

        public async Task<InspectionReportResponseDto?> GetByIdAsync(int id)
        {
            var ir = await _context.InspectionReports
                .Include(x => x.Bike)
                .Include(x => x.Inspector)
                .FirstOrDefaultAsync(x => x.ReportId == id);

            if (ir == null) return null;

            return new InspectionReportResponseDto
            {
                ReportId = ir.ReportId,
                BikeId = ir.BikeId,
                BikeTitle = ir.Bike.Title,
                InspectorId = ir.InspectorId,
                InspectorName = ir.Inspector.FullName,
                FrameCondition = ir.FrameCondition,
                BrakeCondition = ir.BrakeCondition,
                DrivetrainCondition = ir.DrivetrainCondition,
                OverallComment = ir.OverallComment,
                ReportFile = ir.ReportFile,
                InspectionStatus = ir.InspectionStatus.ToString(),
                InspectedAt = ir.InspectedAt
            };
        }

        public async Task<InspectionReportResponseDto> CreateAsync(CreateInspectionReportDto dto)
        {
            var report = new InspectionReport
            {
                BikeId = dto.BikeId,
                InspectorId = dto.InspectorId,
                FrameCondition = dto.FrameCondition,
                BrakeCondition = dto.BrakeCondition,
                DrivetrainCondition = dto.DrivetrainCondition,
                OverallComment = dto.OverallComment,
                ReportFile = dto.ReportFile,
                InspectionStatus = InspectionStatus.PENDING,
                InspectedAt = DateTime.UtcNow
            };

            _context.InspectionReports.Add(report);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(report.ReportId) ?? throw new Exception("Failed to create inspection report");
        }

        public async Task<bool> UpdateAsync(int id, UpdateInspectionReportDto dto)
        {
            var report = await _context.InspectionReports.FindAsync(id);
            if (report == null) return false;

            report.FrameCondition = dto.FrameCondition;
            report.BrakeCondition = dto.BrakeCondition;
            report.DrivetrainCondition = dto.DrivetrainCondition;
            report.OverallComment = dto.OverallComment;
            report.ReportFile = dto.ReportFile;

            if (dto.InspectionStatus != null && Enum.TryParse<InspectionStatus>(dto.InspectionStatus, out var status))
                report.InspectionStatus = status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var report = await _context.InspectionReports.FindAsync(id);
            if (report == null) return false;

            _context.InspectionReports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
