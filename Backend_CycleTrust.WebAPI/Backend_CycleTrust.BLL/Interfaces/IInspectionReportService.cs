using Backend_CycleTrust.BLL.DTOs.InspectionReportDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IInspectionReportService
    {
        Task<IEnumerable<InspectionReportResponseDto>> GetAllAsync();
        Task<IEnumerable<InspectionReportResponseDto>> GetPendingRequestsAsync();
        Task<InspectionReportResponseDto?> GetByIdAsync(int id);
        Task<InspectionReportResponseDto> RequestInspectionAsync(int bikeId, int sellerId);
        Task<InspectionReportResponseDto> CreateAsync(CreateInspectionReportDto dto);
        Task<InspectionReportResponseDto> CompleteInspectionAsync(int reportId, int inspectorId, CompleteInspectionReportDto dto);
        Task<bool> UpdateAsync(int id, UpdateInspectionReportDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
