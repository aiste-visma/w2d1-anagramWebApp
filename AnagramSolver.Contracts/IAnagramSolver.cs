using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IAnagramSolver
    {
        Task<IList<string>> GetAnagramsAsync(string userInput, Action<string> logger, CancellationToken ct);
    }
}
