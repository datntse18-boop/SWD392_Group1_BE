using Backend_CycleTrust.BLL.DTOs.ChatbotDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("suggest")]
        public async Task<IActionResult> SuggestBikes([FromBody] ChatbotSuggestRequestDto request)
        {
            try
            {
                var result = await _chatbotService.SuggestBikesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while suggesting bicycles.", Error = ex.Message });
            }
        }
    }
}
