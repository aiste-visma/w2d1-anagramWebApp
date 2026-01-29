using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramFinder : IAnagramSolver
    {
        private IWordRepository wordRepository;
        private int maxAnagramCount;

        public AnagramFinder(IWordRepository repo, int maxAnagramCount) 
        {
            this.wordRepository = repo;
            this.maxAnagramCount = maxAnagramCount;
        }

        public IList<string> GetAnagrams(string userInput)
        {
            IEnumerable<string> dictionary = wordRepository.GetDictionary();

            List<string> candidates = new();
            foreach (string word in dictionary)
            {
                if (word.Length <= userInput.Length)
                {
                    candidates.Add(word);
                }
            }

            int anagramCount = 0;
            List<string> anagrams = new();

            char[] a = userInput.ToLower().ToCharArray();
            Array.Sort(a);

            foreach(string word in candidates)
            {
                char[] b = word.ToLower().ToCharArray();
                Array.Sort(b);

                bool same = a.SequenceEqual(b);

                if (same && anagramCount < maxAnagramCount)
                {
                    anagrams.Add(word);
                    anagramCount++;
                }
            }
            return anagrams;
        }

    }
}
