using Backend_CycleTrust.BLL.DTOs.ReportDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportResponseDto>> GetAllAsync();
        Task<ReportResponseDto?> GetByIdAsync(int id);
        Task<ReportResponseDto> CreateAsync(CreateReportDto dto);
        Task<bool> UpdateAsync(int id, UpdateReportDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
