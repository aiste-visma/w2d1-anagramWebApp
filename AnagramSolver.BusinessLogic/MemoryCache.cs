using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class MemoryCache<T>
    {
        private Dictionary<string, T> _cache = new();

        public bool TryGet(string word, out T value)
        {
            return _cache.TryGetValue(word, out value);
        }

        public void AddToCache(string word, T anagrams)
        {
            _cache[word] = anagrams;
        }
    }
}
