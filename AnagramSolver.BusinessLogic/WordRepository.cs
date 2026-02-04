using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class WordRepository : IWordRepository
    {
        private string dictionaryPath;
        public WordRepository(string dicPath)
        {
            this.dictionaryPath = dicPath;
        }

        public async Task<IEnumerable<string>> GetDictionary(CancellationToken ct)
        {
            var dictionary = await File.ReadAllLinesAsync(dictionaryPath, ct);
            return dictionary;
        }

    }
}
