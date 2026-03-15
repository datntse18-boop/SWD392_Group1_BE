using Backend_CycleTrust.BLL.DTOs.MessageDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly CycleTrustDbContext _context;
        private readonly IMessageRepository _messageRepository;

        public MessageService(CycleTrustDbContext context, IMessageRepository messageRepository)
        {
            _context = context;
            _messageRepository = messageRepository;
        }

        private static MessageResponseDto MapToDto(Message message)
        {
            return new MessageResponseDto
            {
                MessageId = message.MessageId,
                SenderId = message.SenderId,
                SenderName = message.Sender?.FullName,
                ReceiverId = message.ReceiverId,
                ReceiverName = message.Receiver?.FullName,
                BikeId = message.BikeId,
                Content = message.Content,
                SentAt = message.SentAt
            };
        }

        public async Task<IEnumerable<MessageResponseDto>> GetAllAsync()
        {
            var messages = await _messageRepository.GetAllWithUsersAsync();
            return messages.Select(MapToDto);
        }

        public async Task<MessageResponseDto?> GetByIdAsync(int id)
        {
            var msg = await _messageRepository.GetByIdWithUsersAsync(id);

            if (msg == null) return null;

            return MapToDto(msg);
        }

        public async Task<MessageResponseDto> CreateAsync(CreateMessageDto dto)
        {
            if (dto.SenderId == dto.ReceiverId)
                throw new InvalidOperationException("Sender and receiver must be different users.");

            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new InvalidOperationException("Message content is required.");

            var senderExists = await _context.Users.AnyAsync(user => user.UserId == dto.SenderId);
            if (!senderExists)
                throw new InvalidOperationException("Sender not found.");

            var receiverExists = await _context.Users.AnyAsync(user => user.UserId == dto.ReceiverId);
            if (!receiverExists)
                throw new InvalidOperationException("Receiver not found.");

            if (dto.BikeId.HasValue)
            {
                var bikeExists = await _context.Bikes.AnyAsync(bike => bike.BikeId == dto.BikeId.Value);
                if (!bikeExists)
                    throw new InvalidOperationException("Bike not found.");
            }

            var message = new Message
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                BikeId = dto.BikeId,
                Content = dto.Content.Trim(),
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(message);

            return await GetByIdAsync(message.MessageId) ?? throw new Exception("Failed to create message");
        }

        public async Task<IEnumerable<MessageResponseDto>> GetConversationAsync(int user1Id, int user2Id, int? bikeId)
        {
            var usersExist = await _context.Users
                .CountAsync(user => user.UserId == user1Id || user.UserId == user2Id);

            if (usersExist < 2)
                throw new InvalidOperationException("One or both users were not found.");

            if (bikeId.HasValue)
            {
                var bikeExists = await _context.Bikes.AnyAsync(bike => bike.BikeId == bikeId.Value);
                if (!bikeExists)
                    throw new InvalidOperationException("Bike not found.");
            }

            var messages = await _messageRepository.GetConversationAsync(user1Id, user2Id, bikeId);
            return messages.Select(MapToDto);
        }

        public async Task<IEnumerable<InboxMessageDto>> GetInboxAsync(int userId)
        {
            var userExists = await _context.Users.AnyAsync(user => user.UserId == userId);
            if (!userExists)
                throw new InvalidOperationException("User not found.");

            var messages = await _messageRepository.GetInboxAsync(userId);

            return messages
                .GroupBy(message => new
                {
                    OtherUserId = message.SenderId == userId ? message.ReceiverId : message.SenderId,
                    BikeId = message.BikeId
                })
                .Select(group => group
                    .OrderByDescending(message => message.SentAt)
                    .First())
                .OrderByDescending(message => message.SentAt)
                .Select(message => new InboxMessageDto
                {
                    OtherUserId = message.SenderId == userId ? message.ReceiverId : message.SenderId,
                    OtherUserName = message.SenderId == userId ? message.Receiver?.FullName : message.Sender?.FullName,
                    BikeId = message.BikeId,
                    BikeTitle = message.Bike?.Title,
                    LastMessageId = message.MessageId,
                    LastSenderId = message.SenderId,
                    LastMessageContent = message.Content,
                    LastSentAt = message.SentAt
                });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var message = await _messageRepository.GetByIdAsync(id);
            if (message == null) return false;

            await _messageRepository.DeleteAsync(message);
            return true;
        }
    }
}
