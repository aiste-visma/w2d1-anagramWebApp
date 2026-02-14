using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramCacheDecorator : IAnagramSolver
    {
        private Dictionary<string, IList<string>> _cache = new();
        IAnagramSolver _solver;

        public AnagramCacheDecorator(IAnagramSolver solver) 
        { 
            _solver = solver;
        }
        public async Task<IList<string>> GetAnagramsAsync(string userInput, Action<string> logger, CancellationToken ct)
        {
            IList<string> value;
            if (_cache.TryGetValue(userInput, out value))
            {
                return value;
            }
            var anagrams = await _solver.GetAnagramsAsync(userInput, logger, ct);
            _cache[userInput] = anagrams;
            return anagrams;
        }

    }
}
