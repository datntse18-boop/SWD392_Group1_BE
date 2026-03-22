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

        // FR-11: View Inspection History by Bike
        Task<IEnumerable<InspectionReportResponseDto>> GetByBikeIdAsync(int bikeId);

        // FR-12: Request Re-Inspection
        Task<InspectionReportResponseDto> RequestReInspectionAsync(int reportId, int inspectorId);
    }
}
