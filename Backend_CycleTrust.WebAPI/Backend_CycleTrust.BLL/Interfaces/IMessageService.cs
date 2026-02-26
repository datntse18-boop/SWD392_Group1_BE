using Backend_CycleTrust.BLL.DTOs.MessageDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageResponseDto>> GetAllAsync();
        Task<MessageResponseDto?> GetByIdAsync(int id);
        Task<MessageResponseDto> CreateAsync(CreateMessageDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
