using AnagramSolver.Contracts;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        public async Task<IActionResult> Index(string? id, CancellationToken ct)
        {
            var model = new AnagramViewModel();
            string cleanId;
            if (!string.IsNullOrWhiteSpace(id))
            {
                cleanId = id.Replace(" ", "").ToLower();
                model.userInput = cleanId;
                model.anagrams = (await _anagramSolver.GetAnagramsAsync(cleanId, ct)).ToList();
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
