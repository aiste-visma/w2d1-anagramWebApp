using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class MultipleAnagramFinder : IAnagramSolver
    {
        private List<string> dictionary;

        public MultipleAnagramFinder(IWordRepository repo)
        {

            dictionary = new List<string>();
            foreach (string word in repo.GetDictionary())
            {
                dictionary.Add(word.ToLower());
            }
        }

        public IList<string> GetAnagrams(string userInput)
        {
            LetterBag bag = new LetterBag(userInput);
            var currentSolution = new List<string>();
            var result = new List<string>();

            var filteredDic = new List<string>();
            foreach (string word in dictionary)
            {
                //pridet nauja logika su min output ilgiu

                if (bag.CanWordForm(word))
                {
                    filteredDic.Add(word);
                }
            }

            FindAnagrams(bag, currentSolution, filteredDic, 0, result);
            return result;
        }


        private void FindAnagrams(LetterBag bag, List<string> solution, List<string> dict, int startIndex, List<string> results)
        {
            if (bag.IsEmpty)
            {
                string anagram = string.Join(" ", solution);
                results.Add(anagram);
                return;
            }

            for (int i = startIndex; i < dict.Count; i++)
            {
                string word = dict[i];

                if(!bag.CanWordForm(word))
                    continue;

                bag.RemoveWord(word);
                solution.Add(word);

                FindAnagrams(bag, solution, dict, i, results);

                solution.RemoveAt(solution.Count - 1);
                bag.AddWord(word);
            }

        }
    }
}
