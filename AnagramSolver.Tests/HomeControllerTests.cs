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
//        public void Index_EmptyInput_AnagramSolverNotCalled()
//        {
//            var anagramsMock = new Mock<IAnagramSolver>();
//            var logger = Mock.Of<ILogger<HomeController>>();
//            var options = Options.Create(new AppSettings { MinOutputWordLength = 3 });

//            var controller = new HomeController(logger, anagramsMock.Object, options);

//            controller.Index("");

//            anagramsMock.Verify(m => m.GetAnagrams(It.IsAny<string>(), It.IsAny<int>()), Times.Never);

//        }
//    }
//}
