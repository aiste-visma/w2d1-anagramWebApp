using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly IFrequencyAnalysisService _frequencyService;

        public AnalysisController(IFrequencyAnalysisService frequencyService)
        {
            _frequencyService = frequencyService;
        }

        // POST api/analysis/frequency
        [HttpPost("frequency")]
        [RequestSizeLimit(1_048_576)] // 1 MB
        public IActionResult Frequency([FromBody] FrequencyRequest request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Input cannot be empty.");

            var result = _frequencyService.Analyse(request.Text);
            return Ok(result);
        }
    }
}
