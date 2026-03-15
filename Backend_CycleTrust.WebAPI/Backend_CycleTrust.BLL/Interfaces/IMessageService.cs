using Backend_CycleTrust.BLL.DTOs.MessageDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageResponseDto>> GetAllAsync();
        Task<MessageResponseDto?> GetByIdAsync(int id);
        Task<MessageResponseDto> CreateAsync(CreateMessageDto dto);
        Task<IEnumerable<MessageResponseDto>> GetConversationAsync(int user1Id, int user2Id, int? bikeId);
        Task<IEnumerable<InboxMessageDto>> GetInboxAsync(int userId);
        Task<bool> DeleteAsync(int id);
    }
}
