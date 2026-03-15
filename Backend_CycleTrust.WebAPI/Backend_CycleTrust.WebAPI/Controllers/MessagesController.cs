using Backend_CycleTrust.BLL.DTOs.MessageDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageResponseDto>>> GetAll()
        {
            var messages = await _messageService.GetAllAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MessageResponseDto>> GetById(int id)
        {
            var message = await _messageService.GetByIdAsync(id);
            if (message == null) return NotFound();
            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponseDto>> Create(CreateMessageDto dto)
        {
            try
            {
                var message = await _messageService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = message.MessageId }, message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("conversation")]
        public async Task<ActionResult<IEnumerable<MessageResponseDto>>> GetConversation(
            [FromQuery] int user1Id,
            [FromQuery] int user2Id,
            [FromQuery] int? bikeId)
        {
            try
            {
                var conversation = await _messageService.GetConversationAsync(user1Id, user2Id, bikeId);
                return Ok(conversation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("inbox/{userId}")]
        public async Task<ActionResult<IEnumerable<InboxMessageDto>>> GetInbox(int userId)
        {
            try
            {
                var inbox = await _messageService.GetInboxAsync(userId);
                return Ok(inbox);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _messageService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
