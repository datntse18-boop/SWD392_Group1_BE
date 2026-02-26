using Backend_CycleTrust.BLL.DTOs.MessageDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly CycleTrustDbContext _context;

        public MessageService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageResponseDto>> GetAllAsync()
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Select(m => new MessageResponseDto
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.FullName,
                    ReceiverId = m.ReceiverId,
                    ReceiverName = m.Receiver.FullName,
                    BikeId = m.BikeId,
                    Content = m.Content,
                    SentAt = m.SentAt
                })
                .ToListAsync();
        }

        public async Task<MessageResponseDto?> GetByIdAsync(int id)
        {
            var msg = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefaultAsync(m => m.MessageId == id);

            if (msg == null) return null;

            return new MessageResponseDto
            {
                MessageId = msg.MessageId,
                SenderId = msg.SenderId,
                SenderName = msg.Sender.FullName,
                ReceiverId = msg.ReceiverId,
                ReceiverName = msg.Receiver.FullName,
                BikeId = msg.BikeId,
                Content = msg.Content,
                SentAt = msg.SentAt
            };
        }

        public async Task<MessageResponseDto> CreateAsync(CreateMessageDto dto)
        {
            var message = new Message
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                BikeId = dto.BikeId,
                Content = dto.Content,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(message.MessageId) ?? throw new Exception("Failed to create message");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
