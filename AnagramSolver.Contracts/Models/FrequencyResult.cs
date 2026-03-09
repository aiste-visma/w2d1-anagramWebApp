namespace AnagramSolver.Contracts.Models
{
    public class FrequencyResult
    {
        public List<WordFrequency> TopWords        { get; set; } = new();
        public int                 TotalWordCount  { get; set; }
        public int                 UniqueWordCount { get; set; }
        public string              LongestWord     { get; set; } = string.Empty;
    }
}
