using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IWordRepository
    {
        Task<IEnumerable<string>> GetDictionary(CancellationToken ct);
        Task SaveDictionary(IEnumerable<string> words, CancellationToken ct);
    }
}
