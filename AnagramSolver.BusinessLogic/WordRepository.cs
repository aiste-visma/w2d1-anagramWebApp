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
        private string[] zodynas;
        public WordRepository(string zodynasPath)
        {
            zodynas = File.ReadAllLines(zodynasPath);
        }

        public IEnumerable<string> GetDictionary()
        {
            return zodynas;
        }

    }
}
