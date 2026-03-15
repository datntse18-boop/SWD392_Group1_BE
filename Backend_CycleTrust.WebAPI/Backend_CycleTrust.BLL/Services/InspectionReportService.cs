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

        private static InspectionReportResponseDto MapToDto(InspectionReport ir)
        {
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

        public async Task<IEnumerable<InspectionReportResponseDto>> GetAllAsync()
        {
            var reports = await _context.InspectionReports
                .Include(ir => ir.Bike)
                .Include(ir => ir.Inspector)
                .OrderByDescending(ir => ir.InspectedAt)
                .ToListAsync();

            return reports.Select(MapToDto);
        }

        public async Task<IEnumerable<InspectionReportResponseDto>> GetPendingRequestsAsync()
        {
            var reports = await _context.InspectionReports
                .Include(ir => ir.Bike)
                .Include(ir => ir.Inspector)
                .Where(ir => ir.InspectionStatus == InspectionStatus.PENDING)
                .OrderByDescending(ir => ir.InspectedAt)
                .ToListAsync();

            return reports.Select(MapToDto);
        }

        public async Task<InspectionReportResponseDto?> GetByIdAsync(int id)
        {
            var ir = await _context.InspectionReports
                .Include(x => x.Bike)
                .Include(x => x.Inspector)
                .FirstOrDefaultAsync(x => x.ReportId == id);

            return ir == null ? null : MapToDto(ir);
        }

        public async Task<InspectionReportResponseDto> RequestInspectionAsync(int bikeId, int sellerId)
        {
            var bike = await _context.Bikes.FindAsync(bikeId);
            if (bike == null)
                throw new InvalidOperationException("Bike not found.");

            if (bike.SellerId != sellerId)
                throw new InvalidOperationException("Seller can only request inspection for their own listing.");

            var hasPending = await _context.InspectionReports
                .AnyAsync(x => x.BikeId == bikeId && x.InspectionStatus == InspectionStatus.PENDING);

            if (hasPending)
                throw new InvalidOperationException("This bike already has a pending inspection request.");

            var defaultInspector = await _context.Users
                .FirstOrDefaultAsync(u => u.RoleId == 4);

            if (defaultInspector == null)
                throw new InvalidOperationException("No inspector available to receive inspection request.");

            var report = new InspectionReport
            {
                BikeId = bikeId,
                InspectorId = defaultInspector.UserId,
                OverallComment = "Inspection requested by seller.",
                InspectionStatus = InspectionStatus.PENDING,
                InspectedAt = DateTime.UtcNow
            };

            _context.InspectionReports.Add(report);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(report.ReportId) ?? throw new Exception("Failed to create inspection request");
        }

        public async Task<InspectionReportResponseDto> CreateAsync(CreateInspectionReportDto dto)
        {
            var inspector = await _context.Users.FindAsync(dto.InspectorId);
            if (inspector == null || inspector.RoleId != 4)
                throw new InvalidOperationException("Chỉ user có role INSPECTOR mới được tạo báo cáo kiểm định.");

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

        public async Task<InspectionReportResponseDto> CompleteInspectionAsync(int reportId, int inspectorId, CompleteInspectionReportDto dto)
        {
            var report = await _context.InspectionReports
                .Include(x => x.Bike)
                .Include(x => x.Inspector)
                .FirstOrDefaultAsync(x => x.ReportId == reportId);

            if (report == null)
                throw new InvalidOperationException("Inspection request not found.");

            if (report.InspectionStatus != InspectionStatus.PENDING)
                throw new InvalidOperationException("Only pending inspection requests can be completed.");

            var normalizedResult = dto.Result?.Trim().ToLowerInvariant();
            var status = normalizedResult switch
            {
                "passed" or "pass" => InspectionStatus.APPROVED,
                "failed" or "fail" => InspectionStatus.REJECTED,
                _ => throw new InvalidOperationException("Invalid inspection result. Use 'passed' or 'failed'.")
            };

            report.InspectorId = inspectorId;
            report.FrameCondition = dto.FrameCondition;
            report.BrakeCondition = dto.BrakeCondition;
            report.DrivetrainCondition = dto.DrivetrainCondition;
            report.OverallComment = dto.OverallComment;
            report.ReportFile = dto.ReportFile;
            report.InspectionStatus = status;
            report.InspectedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _context.Entry(report).Reference(x => x.Inspector).LoadAsync();
            return MapToDto(report);
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

            if (dto.InspectionStatus != null && Enum.TryParse<InspectionStatus>(dto.InspectionStatus, ignoreCase: true, out var status))
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
