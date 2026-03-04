using System.Collections.Concurrent;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AnagramSolver.BusinessLogic
{
    public class ChatHistoryStorage
    {
        private readonly ConcurrentDictionary<string, ChatHistory> _histories = new();

        public ChatHistory GetOrCreateHistory(string sessionId, out bool isNew)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentException("SessionId must be provided.", nameof(sessionId));
            }

            if (_histories.TryGetValue(sessionId, out var existingHistory))
            {
                isNew = false;
                return existingHistory;
            }

            var newHistory = new ChatHistory();

            if (_histories.TryAdd(sessionId, newHistory))
            {
                isNew = true;
                return newHistory;
            }

            isNew = false;
            return _histories[sessionId];
        }
    }
}
