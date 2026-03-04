namespace AnagramSolver.Contracts.Models
{
    public class ChatRequest
    {
        public string message { get; set; } = string.Empty;
        public string sessionId { get; set; } = string.Empty;
    }
}