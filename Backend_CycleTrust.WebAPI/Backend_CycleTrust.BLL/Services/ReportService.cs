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
        private readonly ICloudinaryService _cloudinaryService;

        public ReportService(CycleTrustDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        private static ReportResponseDto MapToDto(Report report)
        {
            return new ReportResponseDto
            {
                ReportId = report.ReportId,
                ReporterId = report.ReporterId,
                ReporterName = report.Reporter.FullName,
                BikeId = report.BikeId,
                BikeTitle = report.Bike?.Title,
                SellerId = report.Bike?.SellerId,
                SellerName = report.Bike?.Seller?.FullName,
                Reason = report.Reason,
                ImageUrls = report.ImageUrls,
                Status = report.Status.ToString(),
                CreatedAt = report.CreatedAt
            };
        }

        public async Task<IEnumerable<ReportResponseDto>> GetAllReportsAsync()
        {
            var reports = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Bike)
                .ThenInclude(b => b!.Seller)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reports.Select(MapToDto);
        }

        public async Task<IEnumerable<ReportResponseDto>> GetReportsByReporterAsync(int reporterId)
        {
            var reports = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Bike)
                .ThenInclude(b => b!.Seller)
                .Where(r => r.ReporterId == reporterId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reports.Select(MapToDto);
        }

        public async Task<ReportResponseDto?> GetReportByIdAsync(int id)
        {
            var report = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Bike)
                .ThenInclude(b => b!.Seller)
                .FirstOrDefaultAsync(r => r.ReportId == id);

            return report == null ? null : MapToDto(report);
        }

        public async Task<ReportResponseDto> CreateReportAsync(CreateReportDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new InvalidOperationException("Reason is required.");

            var reporterExists = await _context.Users.AnyAsync(u => u.UserId == dto.ReporterId && u.RoleId == 2);
            if (!reporterExists)
                throw new InvalidOperationException("Reporter must be an existing BUYER.");

            if (dto.BikeId.HasValue)
            {
                var bikeExists = await _context.Bikes.AnyAsync(b => b.BikeId == dto.BikeId.Value);
                if (!bikeExists)
                    throw new InvalidOperationException("Bike not found.");
            }

            var imageUrls = dto.ImageUrls?.Where(url => !string.IsNullOrWhiteSpace(url)).Select(url => url.Trim()).ToList()
                ?? new List<string>();

            if (dto.EvidenceImages != null && dto.EvidenceImages.Count > 0)
            {
                var uploadedUrls = await _cloudinaryService.UploadImagesAsync(dto.EvidenceImages, "reports");
                imageUrls.AddRange(uploadedUrls);
            }

            var report = new Report
            {
                ReporterId = dto.ReporterId,
                BikeId = dto.BikeId,
                Reason = dto.Reason.Trim(),
                ImageUrls = imageUrls.Count > 0 ? imageUrls : null,
                Status = ReportStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return await GetReportByIdAsync(report.ReportId) ?? throw new Exception("Failed to create report");
        }

        public async Task<bool> UpdateOwnPendingReportAsync(int reportId, int reporterId, UpdateOwnReportDto dto)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null)
                return false;

            if (report.ReporterId != reporterId)
                throw new InvalidOperationException("You can only edit your own report.");

            if (report.Status != ReportStatus.Pending)
                throw new InvalidOperationException("Only pending reports can be edited.");

            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new InvalidOperationException("Reason is required.");

            var imageUrls = dto.ImageUrls?.Where(url => !string.IsNullOrWhiteSpace(url)).Select(url => url.Trim()).ToList()
                ?? new List<string>();

            if (dto.EvidenceImages != null && dto.EvidenceImages.Count > 0)
            {
                var uploadedUrls = await _cloudinaryService.UploadImagesAsync(dto.EvidenceImages, "reports");
                imageUrls.AddRange(uploadedUrls);
            }

            report.Reason = dto.Reason.Trim();
            report.ImageUrls = imageUrls.Count > 0 ? imageUrls : null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOwnPendingReportAsync(int reportId, int reporterId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null)
                return false;

            if (report.ReporterId != reporterId)
                throw new InvalidOperationException("You can only delete your own report.");

            if (report.Status != ReportStatus.Pending)
                throw new InvalidOperationException("Only pending reports can be deleted.");

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateReportStatusAsync(int id, UpdateReportDto dto)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
                return false;

            if (!Enum.TryParse<ReportStatus>(dto.Status, ignoreCase: true, out var status))
                throw new InvalidOperationException("Invalid status. Allowed values: Pending, Approved, Rejected.");

            report.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
