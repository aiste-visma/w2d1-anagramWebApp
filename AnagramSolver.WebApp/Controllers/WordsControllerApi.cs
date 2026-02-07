using Microsoft.AspNetCore.Mvc;
using AnagramSolver.Contracts;

namespace AnagramSolver.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class WordsApiController : ControllerBase
    {
        private readonly IWordRepository _repo;

        public WordsApiController(IWordRepository repo)
        {
            _repo = repo; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAll()
        {
            var cancellationToken = HttpContext.RequestAborted;
            var dictionary = await _repo.GetDictionary(cancellationToken);
            return Ok(dictionary);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            var cancellationToken = HttpContext.RequestAborted;
            var dictionary = await _repo.GetDictionary(cancellationToken);
            var allWords = dictionary.ToList();
            return Ok(allWords[id]);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string word)
        {
            var cancellationToken = HttpContext.RequestAborted;
            var dictionary = await _repo.GetDictionary(cancellationToken);
            var allWords = dictionary.ToList();

            if (word != null && !allWords.Contains(word.ToLower()))
            {
                allWords.Add(word);
            }

            await _repo.SaveDictionary(allWords, cancellationToken);
            return Ok(word);
        }

        [HttpDelete("{word}")]
        public async Task<ActionResult> Remove(string word)
        {
            var cancellationToken = HttpContext.RequestAborted;
            var dictionary = await _repo.GetDictionary(cancellationToken);
            var allWords = dictionary.ToList();

            if (word != null && allWords.Contains(word.ToLower()))
            {
                allWords.Remove(word.ToLower());
            }

            await _repo.SaveDictionary(allWords, cancellationToken);

            return Ok(word);
        }
    }
}
