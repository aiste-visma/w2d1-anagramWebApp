//using AnagramSolver.Contracts;
//using AnagramSolver.BusinessLogic;
//using FluentAssertions;
//using Moq;


//namespace AnagramSolver.Tests
//{
//    public class MultipleAnagramFinderTest
//    {
//        [Fact]
//        public void Constructor_GetDictionaryOnce()
//        {
//            //arrange
//            var repositoryMock = new Mock<IWordRepository>();
//            repositoryMock.Setup(r => r.GetDictionary()).Returns(new List<string>());

//            //act
//            var temp = new MultipleAnagramFinder(repositoryMock.Object);

//            //assert
//            repositoryMock.Verify(r => r.GetDictionary(), Times.Once);
//        }

//        [Fact]
//        public void GetAnagrams_OnePossibleAnagram_OneAnagram()
//        {
//            //arrange
//            var repositoryMock = new Mock<IWordRepository>();
//            repositoryMock.Setup(r => r.GetDictionary()).Returns(new List<string> { "kalnas", "berti", "rūkas" });

//            var finder = new MultipleAnagramFinder(repositoryMock.Object);

//            //act
//            var result = finder.GetAnagrams("klanas");

//            //assert
//            result.Should().Equal(new List<string> { "kalnas"});

//        }

//        [Fact]
//        public void GetAnagrams_NoAnagrams_EmptyResult()
//        {
//            //arrange
//            var repositoryMock = new Mock<IWordRepository>();
//            repositoryMock.Setup(r => r.GetDictionary()).Returns(new List<string> {"kalnas", "berti", "rūkas"});

//            var finder = new MultipleAnagramFinder(repositoryMock.Object);

//            //act
//            var result = finder.GetAnagrams("vienas");

//            //assert
//            result.Should().BeEmpty();
//        }

//        [Fact]
//        public void GetAnagrams_MultipleAnagrams_CorrectAnagrams()
//        {
//            //arrange
//            var repositoryMock = new Mock<IWordRepository>();
//            repositoryMock.Setup(r => r.GetDictionary()).Returns(new List<string> { "kalnas", "kas", "lan", "rūkas" });

//            var finder = new MultipleAnagramFinder(repositoryMock.Object);

//            //act
//            var result = finder.GetAnagrams("kalnas");

//            //assert
//            result.Should().BeEquivalentTo(new List<string> { "kalnas", "kas lan" });
//        }

//        [Fact]
//        public void GetAnagrams_InputUppercase_CorrectAnagram()
//        {
//            //arrange
//            var repositoryMock = new Mock<IWordRepository>();
//            repositoryMock.Setup(r => r.GetDictionary()).Returns(new List<string> { "kalnas", "berti", "rūkas" });

//            var finder = new MultipleAnagramFinder(repositoryMock.Object);

//            //act
//            var result = finder.GetAnagrams("KLANAS");

//            //assert
//            result.Should().Equal(new List<string> { "kalnas" });
//        }

//        //TDD
//        [Fact]
//        public void GetAnagrams_MinOutputWordLength0_AllAnagrams()
//        {
//            var repositoryMock = new Mock<IWordRepository>();
//            repositoryMock.Setup(r => r.GetDictionary()).Returns(new List<string> { "kalnas", "kas", "lan", "rūkas" });

//            var finder = new MultipleAnagramFinder(repositoryMock.Object);

//            var result = finder.GetAnagrams("klanas", 0);

//            result.Should().BeEquivalentTo(new List<string> { "kalnas", "kas lan" });

//        }

//        [Fact]
//        public void GetAnagrams_MinOutputWordLength5_FilteredAmagrams()
//        {
//            var repositoryMock = new Mock<IWordRepository>();
//            repositoryMock.Setup(r => r.GetDictionary()).Returns(new List<string> { "kalnas", "kas", "lan", "rūkas" });

//            var finder = new MultipleAnagramFinder(repositoryMock.Object);
//            var results = finder.GetAnagrams("klanas", 5);

//            results.Should().Equal(new List<string> { "kalnas" });
//        }
//    }
//}