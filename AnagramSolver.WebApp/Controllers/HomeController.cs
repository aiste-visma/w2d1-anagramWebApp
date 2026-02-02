using AnagramSolver.Contracts;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnagramSolver _anagramSolver;

        public HomeController(ILogger<HomeController> logger, IAnagramSolver anagramSolver)
        {
            _logger = logger;
            _anagramSolver = anagramSolver;
        }

        public IActionResult Index(string? id)
        {
            var model = new AnagramViewModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                model.userInput = id;
                model.anagrams = _anagramSolver.GetAnagrams(id).ToList();
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
