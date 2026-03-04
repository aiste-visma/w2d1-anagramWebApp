using Microsoft.AspNetCore.Mvc;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using AnagramSolver.BusinessLogic;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Linq;

namespace AnagramSolver.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiChatService _chatService;
        private readonly ChatHistoryStorage _chatHistoryStorage;

        public AiController(IAiChatService chatService, ChatHistoryStorage chatHistoryStorage)
        {
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _chatHistoryStorage = chatHistoryStorage ?? throw new ArgumentNullException(nameof(chatHistoryStorage));
        }

        [HttpPost("chat")]
        public async Task<ActionResult<ChatResponse>> Chat([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.message))
            {
                return BadRequest("Message cannot be empty.");
            }

            var sessionId = string.IsNullOrWhiteSpace(request.sessionId)
                ? Guid.NewGuid().ToString()
                : request.sessionId;

            var response = await _chatService.GetResponseAsync(request.message, sessionId);

            var chatResponse = new ChatResponse
            {
                response = response,
                sessionId = sessionId
            };

            return Ok(chatResponse);
        }

        [HttpGet("chat/{sessionId}/history")]
        public ActionResult<ChatHistoryResponse> GetHistory(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return BadRequest("SessionId is required.");
            }

            var history = _chatHistoryStorage.GetOrCreateHistory(sessionId, out _);

            var messages = history
                .Select(m => new ChatHistoryMessage
                {
                    role = m.Role.ToString(),
                    content = m.Content ?? string.Empty
                })
                .ToList();

            var response = new ChatHistoryResponse
            {
                sessionId = sessionId,
                messages = messages
            };

            return Ok(response);
        }
    }
}
