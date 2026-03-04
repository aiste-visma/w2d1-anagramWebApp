using AnagramSolver.Contracts;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AnagramSolver.BusinessLogic
{
    public class AiChatService : IAiChatService
    {
        private readonly Kernel _kernel;
        private readonly ChatHistoryStorage _chatHistoryStorage;

        public AiChatService(Kernel kernel, ChatHistoryStorage chatHistoryStorage)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _chatHistoryStorage = chatHistoryStorage ?? throw new ArgumentNullException(nameof(chatHistoryStorage));
        }

        public async Task<string> GetResponseAsync(string userMessage)
        {
            return await GetResponseAsync(userMessage, Guid.NewGuid().ToString());
        }

        public async Task<string> GetResponseAsync(string userMessage, string sessionId)
        {
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

            var chatHistory = _chatHistoryStorage.GetOrCreateHistory(sessionId, out var isNew);

            if (isNew)
            {
                chatHistory.AddSystemMessage("You are an intelligent assistant specialized in word games and anagrams. " +
                    "You help users find anagrams, understand word patterns, and explore linguistic relationships. " +
                    "Provide clear, helpful responses and ask clarifying questions when needed." + "Use the provided tools");
            }

            chatHistory.AddUserMessage(userMessage);

            var settings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var response = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                settings,
                _kernel);
            
            return response.Content ?? string.Empty;
        }
    }
}
