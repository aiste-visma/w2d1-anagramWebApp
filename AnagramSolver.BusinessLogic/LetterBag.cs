using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class LetterBag
    {
        private Dictionary<char, int> wordLetters = new();

        public LetterBag(string userInput)
        {
            char[] userLetters = userInput.ToLower().ToArray();
            foreach (char letter in userLetters)
            {
                if (wordLetters.ContainsKey(letter))
                {
                    wordLetters[letter]++;
                }
                else
                {
                    wordLetters[letter] = 1;
                }
            }
        }

        public bool CanWordForm(string word)
        {
            Dictionary<char, int> copy = new(wordLetters);

            foreach (char letter in word.ToLower())
            {
                if (!copy.ContainsKey(letter))
                    return false;
                copy[letter]--;
                if (copy[letter] < 0)
                    return false;
            }
            return true;
        }

        public bool IsEmpty => wordLetters.Count == 0;

        public void RemoveWord(string word)
        {
            foreach (char letter in word.ToLower()) {
                if (wordLetters.ContainsKey(letter))
                {
                    wordLetters[letter]--;
                    if (wordLetters[letter] == 0)
                    {
                        wordLetters.Remove(letter);
                    }
                }
            }
        }

        public void AddWord(string word)
        {
            foreach (char letter in word.ToLower())
            {
                if (wordLetters.ContainsKey(letter))
                    wordLetters[letter]++;
                else
                    wordLetters[letter] = 1;
            }
        }
    }
}
