namespace AnagramSolver.BusinessLogic
{
    public static class StopWords
    {
        public static readonly HashSet<string> All = new(StringComparer.Ordinal)
        {
            "the", "a", "an", "is", "are", "was", "were", "be", "been", "being",
            "has", "have", "had", "do", "does", "did", "will", "would", "could",
            "should", "may", "might", "shall", "can", "need", "dare", "ought",
            "used", "i", "you", "he", "she", "it", "we", "they", "me", "him",
            "her", "us", "them", "my", "your", "his", "its", "our", "their",
            "this", "that", "these", "those", "in", "on", "at", "by", "for",
            "of", "with", "to", "from", "and", "or", "but", "not", "no", "so",
            "if", "as", "up", "out", "about", "into", "than", "then", "when",
            "where", "who", "which", "what", "how", "all", "each", "every",
            "both", "few", "more", "most", "other", "some", "such", "own",
            "same", "just", "now", "also"
        };
    }
}
