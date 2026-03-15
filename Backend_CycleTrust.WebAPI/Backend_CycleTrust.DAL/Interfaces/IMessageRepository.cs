using Backend_CycleTrust.DAL.Entities;

namespace Backend_CycleTrust.DAL.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllWithUsersAsync();
        Task<Message?> GetByIdWithUsersAsync(int id);
        Task<Message> AddAsync(Message message);
        Task<IEnumerable<Message>> GetConversationAsync(int user1Id, int user2Id, int? bikeId);
        Task<IEnumerable<Message>> GetInboxAsync(int userId);
        Task<Message?> GetByIdAsync(int id);
        Task DeleteAsync(Message message);
    }
}
