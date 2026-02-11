//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AnagramSolver.Contracts;
//using Castle.Core.Logging;
//using Moq;
//using AnagramSolver.WebApp.Controllers;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.AspNetCore.Mvc;
//using AnagramSolver.WebApp.Models;

//namespace AnagramSolver.Tests
//{
//    public class HomeControllerTests
//    {
//        [Fact]
//        public async Task Index_EmptyInput_AnagramSolverNotCalled()
//        {
//            var ct = CancellationToken.None;
//            var anagramsMock = new Mock<IAnagramSolver>();
//            var logger = Mock.Of<ILogger<HomeController>>();
//            var options = Options.Create(new AppSettings { MinOutputWordLength = 3 });

//            var controller = new HomeController(logger, anagramsMock.Object);

//            await controller.Index("", ct);

//            anagramsMock.Verify(m => m.GetAnagramsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
//        }

//        [Fact]
//        public async Task Index_WordWithAnagrams_CorrectAnagrams() 
//        {
//            var ct = CancellationToken.None;
//            var expectedAnagrams = new List<string> { "mala", "lama" };
//            var anagramsMock = new Mock<IAnagramSolver>();
//            anagramsMock.Setup(s => s.GetAnagramsAsync("alma", ct)).ReturnsAsync(expectedAnagrams);
//            var logger = Mock.Of<ILogger<HomeController>>();
//            var options = Options.Create(new AppSettings { MinOutputWordLength = 3 });

//            var controller = new HomeController(logger, anagramsMock.Object);

//            var result = await controller.Index(" al ma ", ct);

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsType<AnagramViewModel>(viewResult.Model);

//            Assert.Equal("alma", model.userInput);
//            Assert.Equal(expectedAnagrams, model.anagrams);

//            anagramsMock.Verify(s => s.GetAnagramsAsync("alma", ct), Times.Once);
//        }

//        [Fact]
//        public async Task Index_WordWithNoAnagrams_NoAnagrams()
//        {
//            var ct = CancellationToken.None;
//            var expectedAnagrams = new List<string>();
//            var anagramsMock = new Mock<IAnagramSolver>();
//            anagramsMock.Setup(s => s.GetAnagramsAsync("kava", ct)).ReturnsAsync(expectedAnagrams);
//            var logger = Mock.Of<ILogger<HomeController>>();
//            var options = Options.Create(new AppSettings { MinOutputWordLength = 3 });

//            var controller = new HomeController(logger, anagramsMock.Object);

//            var result = await controller.Index("kava", ct);

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsType<AnagramViewModel>(viewResult.Model);

//            Assert.Equal("kava", model.userInput);
//            Assert.Equal(expectedAnagrams, model.anagrams);

//            anagramsMock.Verify(s => s.GetAnagramsAsync("kava", ct), Times.Once);
//        }
//    }
//}
