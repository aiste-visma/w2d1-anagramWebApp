using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;

namespace AnagramSolver.Tests
{
    public class AnalysisControllerTests
    {
        private readonly Mock<IFrequencyAnalysisService> _serviceMock;
        private readonly AnalysisController _sut;

        public AnalysisControllerTests()
        {
            _serviceMock = new Mock<IFrequencyAnalysisService>();
            _sut = new AnalysisController(_serviceMock.Object);
        }

        // 4.2.1
        [Fact]
        public void Frequency_EmptyText_ReturnsBadRequest()
        {
            var request = new FrequencyRequest { Text = "" };

            var result = _sut.Frequency(request);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Input cannot be empty.");
        }

        // 4.2.5
        [Fact]
        public void Frequency_NullRequest_ReturnsBadRequest()
        {
            var result = _sut.Frequency(null!);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        // 4.2.2
        [Fact]
        public void Frequency_WhitespaceText_ReturnsBadRequest()
        {
            var request = new FrequencyRequest { Text = "  " };

            var result = _sut.Frequency(request);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        // 4.2.3
        [Fact]
        public void Frequency_ValidText_ReturnsOkWithResult()
        {
            var expected = new FrequencyResult
            {
                TotalWordCount  = 3,
                UniqueWordCount = 2,
                LongestWord     = "hello",
                TopWords        = new List<WordFrequency> { new() { Word = "hello", Count = 2 } }
            };
            _serviceMock.Setup(s => s.Analyse(It.IsAny<string>())).Returns(expected);
            var request = new FrequencyRequest { Text = "hello hello world" };

            var result = _sut.Frequency(request);

            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(expected);
        }

        // 4.2.4
        [Fact]
        public void Frequency_ValidText_CallsServiceExactlyOnce()
        {
            var text = "some valid text";
            _serviceMock.Setup(s => s.Analyse(text)).Returns(new FrequencyResult());
            var request = new FrequencyRequest { Text = text };

            _sut.Frequency(request);

            _serviceMock.Verify(s => s.Analyse(text), Times.Once);
        }
    }
}
