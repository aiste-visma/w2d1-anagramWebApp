using System.Collections.Generic;

namespace AnagramSolver.Contracts.Models
{
    public class ChatHistoryResponse
    {
        public string sessionId { get; set; } = string.Empty;
        public List<ChatHistoryMessage> messages { get; set; } = new();
    }
}
