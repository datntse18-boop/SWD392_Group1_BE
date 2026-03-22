namespace Backend_CycleTrust.BLL.DTOs.ChatbotDTOs
{
    public class ChatbotSuggestRequestDto
    {
        public double? Weight { get; set; } // in kg
        public double? Height { get; set; } // in cm
        public string? BikeType { get; set; } // e.g. "Mountain", "Road", "Hybrid"
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
    }

    public class ChatbotSuggestResponseDto
    {
        public string Message { get; set; } = null!;
        public List<Backend_CycleTrust.BLL.DTOs.BikeDTOs.BikeResponseDto> SuggestedBikes { get; set; } = new();
    }
}
