using Backend_CycleTrust.BLL.DTOs.ReviewDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponseDto>> GetAllAsync();
        Task<ReviewResponseDto?> GetByIdAsync(int id);
        Task<ReviewResponseDto> CreateAsync(CreateReviewDto dto);
        Task<bool> UpdateAsync(int id, UpdateReviewDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
