using AnagramSolver.Contracts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class LINQwithDictionary
    {
        private IWordRepository _WordRepository;
        public LINQwithDictionary(IWordRepository repo)
        {
            _WordRepository = repo;

        }

        public async Task LINQoperations(CancellationToken ct)
        {
            var dictionary = await _WordRepository.GetDictionary(ct);

            var longestWords = dictionary.OrderBy(word => word.Length).Reverse().Take(10);
            foreach (var word in longestWords)
            {
                Console.WriteLine(word);
            }

            var groupedByFirstLetter = dictionary.GroupBy(w => w[0])
                .ToDictionary(c => c.Key, c => c.Count());
            foreach (var group in groupedByFirstLetter)
            {
                Console.WriteLine($"{group.Key} : {group.Value}");
            }

            var palindromes = dictionary.Where(w => w == string.Join("", w.Reverse()));
            Console.WriteLine("Palindromes: ");
            foreach (var word in palindromes)
            {
                Console.WriteLine(word);
            }

        }
    }
}
