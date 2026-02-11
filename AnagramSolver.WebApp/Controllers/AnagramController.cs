using AnagramSolver.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnagramsController : ControllerBase
    {
        private readonly IAnagramSolver _solver;

        public AnagramsController(IAnagramSolver solver)
        {
            _solver = solver;
        }

        [HttpGet("{word}")]
        public async Task<ActionResult<IEnumerable<string>>> Get(string word)
        {
            var cancellationToken = HttpContext.RequestAborted;
            var anagrams = await _solver.GetAnagramsAsync(word, Console.WriteLine, cancellationToken);
            return Ok(anagrams);
        }
    }

}
