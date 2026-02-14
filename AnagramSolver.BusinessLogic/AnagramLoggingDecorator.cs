using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramLoggingDecorator :IAnagramSolver
    {
        IAnagramSolver _solver;

        public AnagramLoggingDecorator(IAnagramSolver solver)
        {
            _solver = solver;
        }
        public async Task<IList<string>> GetAnagramsAsync(string userInput, Action<string> logger, CancellationToken ct)
        {
            logger($"Begining anagram logging.....");
            var anagrams = await _solver.GetAnagramsAsync(userInput, logger, ct);
            logger($"Finished anagram logging.");
            return anagrams;
        }

    }
}
