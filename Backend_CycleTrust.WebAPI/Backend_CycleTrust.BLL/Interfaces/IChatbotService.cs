using Backend_CycleTrust.BLL.DTOs.ChatbotDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IChatbotService
    {
        Task<ChatbotSuggestResponseDto> SuggestBikesAsync(ChatbotSuggestRequestDto request);
    }
}
