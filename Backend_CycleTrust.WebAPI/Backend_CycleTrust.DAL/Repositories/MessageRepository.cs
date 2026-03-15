using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CycleTrustDbContext _context;

        public MessageRepository(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllWithUsersAsync()
        {
            return await BaseQuery()
                .OrderByDescending(message => message.SentAt)
                .ToListAsync();
        }

        public async Task<Message?> GetByIdWithUsersAsync(int id)
        {
            return await BaseQuery()
                .FirstOrDefaultAsync(message => message.MessageId == id);
        }

        public async Task<Message> AddAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(int user1Id, int user2Id, int? bikeId)
        {
            var query = BaseQuery()
                .Where(message =>
                    (message.SenderId == user1Id && message.ReceiverId == user2Id)
                    || (message.SenderId == user2Id && message.ReceiverId == user1Id));

            if (bikeId.HasValue)
            {
                query = query.Where(message => message.BikeId == bikeId.Value);
            }

            return await query
                .OrderBy(message => message.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetInboxAsync(int userId)
        {
            return await BaseQuery()
                .Where(message => message.SenderId == userId || message.ReceiverId == userId)
                .OrderByDescending(message => message.SentAt)
                .ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task DeleteAsync(Message message)
        {
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Message> BaseQuery()
        {
            return _context.Messages
                .Include(message => message.Sender)
                .Include(message => message.Receiver)
                .Include(message => message.Bike);
        }
    }
}
