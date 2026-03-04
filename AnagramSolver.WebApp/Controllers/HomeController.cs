using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.EF.CodeFirst.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnagramSolver _anagramSolver;
        private InputValidationPipeline _validationPipeline;
        private readonly AnagramDbContext _context;

        public HomeController(ILogger<HomeController> logger, IAnagramSolver anagramSolver, InputValidationPipeline pipeline, AnagramDbContext context)
        {
            _logger = logger;
            _anagramSolver = anagramSolver;
            _validationPipeline = pipeline;
            _context = context;
        }

        public async Task<IActionResult> Index(string? id, CancellationToken ct)
        {
            var historyJson = HttpContext.Session.GetString("searchHistory");
            List<string> history;
            if (historyJson != null)
            {
                history = JsonSerializer.Deserialize<List<string>>(historyJson)!;
            }
            else
            {
                history = new List<string>();
            }

            var model = new AnagramViewModel();

            string cleanId = (id ?? "").Replace(" ", "").ToLower();
            try
            {
                await _validationPipeline.Execute(cleanId);
                Response.Cookies.Append("lastSearch", id!, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(2)
                });
                model.userInput = cleanId;
                model.anagrams = (await _anagramSolver.GetAnagramsAsync(cleanId, Console.WriteLine, ct)).ToList();
                var newLog = new SearchLog
                {
                    SearchText = cleanId,
                    SearchedAt = DateTime.Now
                };
                _context.SearchLogs.Add(newLog);
                await _context.SaveChangesAsync();

                history!.Add(id!);
                HttpContext.Session.SetString("searchHistory", JsonSerializer.Serialize(history));
                }
            catch (ArgumentException ex)
            {
                ViewBag.ValidationError = ex.Message;
            }

            var lastSearch = Request.Cookies["lastSearch"];
            ViewBag.LastSearch = lastSearch;
            ViewBag.History = history;
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
