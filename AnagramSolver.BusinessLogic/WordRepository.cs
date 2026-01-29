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
        private string[] dictionary;
        public WordRepository(string zodynasPath)
        {
            dictionary = File.ReadAllLines(zodynasPath);
        }

        public IEnumerable<string> GetDictionary()
        {
            return dictionary;
        }

    }
}
