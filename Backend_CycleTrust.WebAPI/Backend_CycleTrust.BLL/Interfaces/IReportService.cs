using Backend_CycleTrust.BLL.DTOs.ReportDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportResponseDto>> GetAllReportsAsync();
        Task<IEnumerable<ReportResponseDto>> GetReportsByReporterAsync(int reporterId);
        Task<ReportResponseDto?> GetReportByIdAsync(int id);
        Task<ReportResponseDto> CreateReportAsync(CreateReportDto dto);
        Task<bool> UpdateOwnPendingReportAsync(int reportId, int reporterId, UpdateOwnReportDto dto);
        Task<bool> DeleteOwnPendingReportAsync(int reportId, int reporterId);
        Task<bool> UpdateReportStatusAsync(int id, UpdateReportDto dto);
    }
}
