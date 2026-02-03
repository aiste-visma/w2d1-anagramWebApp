using AnagramSolver.Contracts;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    public class WordsController : Controller
    {
        private readonly IWordRepository _wordRepository;

        public WordsController(IWordRepository wordRepositroy)
        {
            _wordRepository = wordRepositroy;
        }
        public IActionResult Index(int page = 1)
        {
            var model = new PaginationViewModel();
            var pageSize = 100;
            var allItems = _wordRepository.GetDictionary();
            var items = allItems.Skip((page - 1) * pageSize).Take(pageSize);
            int totalPages = allItems.Count() / pageSize;

            model.Items = items.ToList();
            model.CurrentPage = page;
            model.TotalPages = totalPages;
            
            return View(model);
        }
    }
}
