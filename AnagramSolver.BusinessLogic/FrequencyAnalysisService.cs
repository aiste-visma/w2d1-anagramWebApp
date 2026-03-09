using System.Text.RegularExpressions;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic
{
    public class FrequencyAnalysisService : IFrequencyAnalysisService
    {
        private static readonly Regex TokenizerRegex =
            new(@"\p{L}+", RegexOptions.Compiled);

        private const int MaxInputLength = 500_000;

        public FrequencyResult Analyse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new FrequencyResult();

            if (text.Length > MaxInputLength)
                throw new ArgumentOutOfRangeException(nameof(text),
                    $"Input must not exceed {MaxInputLength} characters.");

            var allTokens = Tokenize(text).ToList();
            var totalWordCount = allTokens.Count;

            var filtered = allTokens
                .Where(t => !StopWords.All.Contains(t))
                .ToList();

            if (filtered.Count == 0)
                return new FrequencyResult { TotalWordCount = totalWordCount };

            var uniqueWordCount = filtered.Distinct(StringComparer.OrdinalIgnoreCase).Count();

            var longestWord = filtered.MaxBy(w => w.Length)!;

            var topWords = filtered
                .GroupBy(w => w, StringComparer.OrdinalIgnoreCase)
                .Select(g => new WordFrequency { Word = g.Key, Count = g.Count() })
                .OrderByDescending(wf => wf.Count)
                .ThenBy(wf => wf.Word, StringComparer.OrdinalIgnoreCase)
                .Take(10)
                .ToList();

            return new FrequencyResult
            {
                TopWords        = topWords,
                TotalWordCount  = totalWordCount,
                UniqueWordCount = uniqueWordCount,
                LongestWord     = longestWord
            };
        }

        private static IEnumerable<string> Tokenize(string text)
        {
            return TokenizerRegex.Matches(text)
                .Select(m => m.Value.ToLowerInvariant());
        }
    }
}
