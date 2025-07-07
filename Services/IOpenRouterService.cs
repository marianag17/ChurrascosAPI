namespace ChurrascosAPI.Services
{
    public interface IOpenRouterService
    {
        Task<string> AnalizeDataAsync(string prompt);
        Task<string> GetRecommendationsAsync(RecommendationRequest request);
        Task<string> ChatBotResponseAsync(string userMessage, string conversationHistory = "");
    }

    public class OpenRouterResponse
    {
        public Choice[]? choices { get; set; }
    }

    public class Choice
    {
        public Message? message { get; set; }
    }

    public class Message
    {
        public string? content { get; set; }
    }
}