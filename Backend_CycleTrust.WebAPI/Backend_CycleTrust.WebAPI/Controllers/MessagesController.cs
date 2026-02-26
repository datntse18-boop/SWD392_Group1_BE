using Backend_CycleTrust.BLL.DTOs.MessageDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var message = await _messageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = message.MessageId }, message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _messageService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
