using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IAiChatService
    {
        Task<string> GetResponseAsync(string userMessage);

        Task<string> GetResponseAsync(string userMessage, string sessionId);
    }
}
