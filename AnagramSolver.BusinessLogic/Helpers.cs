using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public static class Helpers
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        public static List<T> Where<T>(IEnumerable<T> source, Predicate<T> condition)
        {
            var results = new List<T>();
            foreach (var item in source)
            {
                if (condition(item))
                {
                    results.Add(item);
                }
            }
            return results;
        }

        public static string lower(string input)
        {
            return input.ToLower();
        }

        public static string upper(string input)
        {
            return input.ToUpper();
        }

        public static string reverse(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


    }
}
