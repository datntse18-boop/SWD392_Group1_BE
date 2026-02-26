using Backend_CycleTrust.BLL.DTOs.InspectionReportDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IInspectionReportService
    {
        Task<IEnumerable<InspectionReportResponseDto>> GetAllAsync();
        Task<InspectionReportResponseDto?> GetByIdAsync(int id);
        Task<InspectionReportResponseDto> CreateAsync(CreateInspectionReportDto dto);
        Task<bool> UpdateAsync(int id, UpdateInspectionReportDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
