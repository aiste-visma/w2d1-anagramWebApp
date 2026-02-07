using AnagramSolver.Contracts;
using AnagramSolver.BusinessLogic;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;


namespace AnagramSolver.Tests
{
    public class MultipleAnagramFinderTest
    {
        [Fact]
        public async Task GetAnagramsAsync_GetDictionaryOnce()
        {
            var ct = CancellationToken.None;
            var fakeSetings = new AppSettings { MinOutputWordLength = 0 };
            var options = Options.Create(fakeSetings);
            var repositoryMock = new Mock<IWordRepository>();
            repositoryMock.Setup(r => r.GetDictionary(ct)).ReturnsAsync(new List<string>());


            var temp = new MultipleAnagramFinder(repositoryMock.Object, options);
            await temp.GetAnagramsAsync("test", ct);

            repositoryMock.Verify(r => r.GetDictionary(ct), Times.Once);
        }

        [Fact]
        public async Task GetAnagrams_OnePossibleAnagram_OneAnagram()
        {
            var ct = CancellationToken.None;
            var fakeSetings = new AppSettings { MinOutputWordLength = 0 };
            var options = Options.Create(fakeSetings);
            var repositoryMock = new Mock<IWordRepository>();
            repositoryMock.Setup(r => r.GetDictionary(ct)).ReturnsAsync(new List<string> { "kalnas", "berti", "rūkas" });

            var finder = new MultipleAnagramFinder(repositoryMock.Object, options);
            var result = await finder.GetAnagramsAsync("klanas", ct);

            result.Should().Equal(new List<string> { "kalnas" });

        }

        [Fact]
        public async Task GetAnagrams_NoAnagrams_EmptyResult()
        {
            var ct = CancellationToken.None;
            var fakeSetings = new AppSettings { MinOutputWordLength = 0 };
            var options = Options.Create(fakeSetings);
            var repositoryMock = new Mock<IWordRepository>();
            repositoryMock.Setup(r => r.GetDictionary(ct)).ReturnsAsync(new List<string> { "kalnas", "berti", "rūkas" });

            var finder = new MultipleAnagramFinder(repositoryMock.Object, options);

            var result = await finder.GetAnagramsAsync("vienas", ct);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAnagrams_MultipleAnagrams_CorrectAnagrams()
        {
            var ct = CancellationToken.None;
            var fakeSetings = new AppSettings { MinOutputWordLength = 0 };
            var options = Options.Create(fakeSetings);
            var repositoryMock = new Mock<IWordRepository>();
            repositoryMock.Setup(r => r.GetDictionary(ct)).ReturnsAsync(new List<string> { "kalnas", "kas", "lan", "rūkas" });

            var finder = new MultipleAnagramFinder(repositoryMock.Object, options);

            var result = await finder.GetAnagramsAsync("kalnas", ct);

            result.Should().BeEquivalentTo(new List<string> { "kalnas", "kas lan" });
        }

        [Fact]
        public async Task GetAnagrams_InputUppercase_CorrectAnagram()
        {
            var ct = CancellationToken.None;
            var fakeSetings = new AppSettings { MinOutputWordLength = 0 };
            var options = Options.Create(fakeSetings);
            var repositoryMock = new Mock<IWordRepository>();
            repositoryMock.Setup(r => r.GetDictionary(ct)).ReturnsAsync(new List<string> { "kalnas", "berti", "rūkas" });

            var finder = new MultipleAnagramFinder(repositoryMock.Object, options);

            var result = await finder.GetAnagramsAsync("KLANAS", ct);

            result.Should().Equal(new List<string> { "kalnas" });
        }

        //TDD
        [Fact]
        public async Task GetAnagrams_MinOutputWordLength0_AllAnagrams()
        {
            var ct = CancellationToken.None;
            var fakeSetings = new AppSettings { MinOutputWordLength = 0 };
            var options = Options.Create(fakeSetings);
            var repositoryMock = new Mock<IWordRepository>();
            repositoryMock.Setup(r => r.GetDictionary(ct)).ReturnsAsync(new List<string> { "kalnas", "kas", "lan", "rūkas" });

            var finder = new MultipleAnagramFinder(repositoryMock.Object, options);

            var result = await finder.GetAnagramsAsync("klanas", ct);

            result.Should().BeEquivalentTo(new List<string> { "kalnas", "kas lan" });

        }

        [Fact]
        public async Task GetAnagrams_MinOutputWordLength5_FilteredAmagrams()
        {
            var ct = CancellationToken.None;
            var fakeSetings = new AppSettings { MinOutputWordLength = 5 };
            var options = Options.Create(fakeSetings);
            var repositoryMock = new Mock<IWordRepository>();
            repositoryMock.Setup(r => r.GetDictionary(ct)).ReturnsAsync(new List<string> { "kalnas", "kas", "lan", "rūkas" });

            var finder = new MultipleAnagramFinder(repositoryMock.Object, options);
            var results = await finder.GetAnagramsAsync("klanas", ct);

            results.Should().Equal(new List<string> { "kalnas" });
        }
    }
}