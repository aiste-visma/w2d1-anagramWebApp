using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Models;
using FluentAssertions;

namespace AnagramSolver.Tests
{
    public class FrequencyAnalysisServiceTests
    {
        private readonly FrequencyAnalysisService _sut = new();

        // 4.1.1
        [Fact]
        public void Analyse_EmptyString_ReturnsEmptyResult()
        {
            var result = _sut.Analyse("");

            result.TopWords.Should().BeEmpty();
            result.TotalWordCount.Should().Be(0);
            result.UniqueWordCount.Should().Be(0);
            result.LongestWord.Should().Be("");
        }

        // 4.1.2
        [Fact]
        public void Analyse_WhitespaceOnly_ReturnsEmptyResult()
        {
            var result = _sut.Analyse("   ");

            result.TopWords.Should().BeEmpty();
            result.TotalWordCount.Should().Be(0);
            result.UniqueWordCount.Should().Be(0);
            result.LongestWord.Should().Be("");
        }

        // 4.1.3
        [Fact]
        public void Analyse_PunctuationOnly_ReturnsEmptyResult()
        {
            var result = _sut.Analyse("!!! --- ...");

            result.TopWords.Should().BeEmpty();
            result.TotalWordCount.Should().Be(0);
            result.UniqueWordCount.Should().Be(0);
            result.LongestWord.Should().Be("");
        }

        // 4.1.4
        [Fact]
        public void Analyse_SingleSignificantWord_ReturnsThatWord()
        {
            var result = _sut.Analyse("hello");

            result.TopWords.Should().ContainSingle(w => w.Word == "hello" && w.Count == 1);
            result.TotalWordCount.Should().Be(1);
            result.UniqueWordCount.Should().Be(1);
            result.LongestWord.Should().Be("hello");
        }

        // 4.1.5
        [Fact]
        public void Analyse_StopWordsOnly_ReturnsZeroTopWords()
        {
            var result = _sut.Analyse("the a is");

            result.TopWords.Should().BeEmpty();
            result.UniqueWordCount.Should().Be(0);
            result.TotalWordCount.Should().Be(3);
        }

        // 4.1.6
        [Fact]
        public void Analyse_MixedCase_CountedCaseInsensitively()
        {
            var result = _sut.Analyse("Fox fox FOX");

            result.TopWords.Should().ContainSingle(w => w.Word == "fox" && w.Count == 3);
        }

        // 4.1.7
        [Fact]
        public void Analyse_ReturnsAtMostTenTopWords()
        {
            var distinctWords = new[]
            {
                "alpha", "bravo", "charlie", "delta", "echo",
                "foxtrot", "golf", "hotel", "india", "juliet",
                "kilo", "lima", "mike", "november", "oscar"
            };
            var words = string.Join(" ", distinctWords);
            var result = _sut.Analyse(words);

            result.TopWords.Count.Should().Be(10);
        }

        // 4.1.8
        [Fact]
        public void Analyse_TiesOrderedAlphabetically()
        {
            var result = _sut.Analyse("banana apple");

            result.TopWords[0].Word.Should().Be("apple");
            result.TopWords[1].Word.Should().Be("banana");
        }

        // 4.1.9
        [Fact]
        public void Analyse_LongestWordCorrect()
        {
            var result = _sut.Analyse("cat elephant");

            result.LongestWord.Should().Be("elephant");
        }

        // 4.1.10 — tokenizer splits "it's" into ["it", "s"]; "it" is a stop word; "s" is retained
        [Fact]
        public void Analyse_SpecialCharacters_OnlyLettersExtracted()
        {
            var result = _sut.Analyse("hello, world! it's great");

            var words = result.TopWords.Select(w => w.Word).ToList();
            words.Should().Contain("hello");
            words.Should().Contain("world");
            // "it's" tokenized as ["it", "s"]; "it" is stop word; "s" is kept
            words.Should().Contain("s");
            // "great" is also a significant word
            words.Should().Contain("great");
        }

        // 4.1.11
        [Fact]
        public void Analyse_UniqueWordCount_ExcludesStopWords()
        {
            var result = _sut.Analyse("the fox and the dog");

            result.UniqueWordCount.Should().Be(2); // fox, dog
            result.TotalWordCount.Should().Be(5);
        }

        // 4.1.12
        [Fact]
        public void Analyse_UnicodeLetters_Handled()
        {
            var result = _sut.Analyse("café naïve");

            var words = result.TopWords.Select(w => w.Word).ToList();
            words.Should().Contain("café");
            words.Should().Contain("naïve");
        }
    }
}
