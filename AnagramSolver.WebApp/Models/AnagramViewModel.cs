namespace AnagramSolver.WebApp.Models
{
    public class AnagramViewModel
    {
        public string userInput { get; set; } = string.Empty;
        public List<string> anagrams { get; set; } = new List<string>();

    }
}
