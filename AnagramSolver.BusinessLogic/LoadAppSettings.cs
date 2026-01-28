using System.Text.Json;
using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class LoadAppSettings
    {
        public static AppSettings FromJson(string jsonPath)
        {
            string jsonText = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<AppSettings>(jsonText)!;
        }
    }
}
