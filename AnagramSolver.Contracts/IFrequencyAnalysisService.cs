using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Contracts
{
    public interface IFrequencyAnalysisService
    {
        /// <summary>
        /// Analyses <paramref name="text"/> and returns word frequency stats.
        /// Returns an empty result (not null) when no valid tokens are found.
        /// </summary>
        FrequencyResult Analyse(string text);
    }
}
