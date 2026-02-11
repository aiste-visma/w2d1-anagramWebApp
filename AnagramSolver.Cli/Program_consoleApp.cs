using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;


class Program {
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        AppSettings settings = LoadAppSettings.FromJson("appsettings.json");
        var zodynas = new WordRepository("zodynas.txt");

        string userInput;
        while (true)
        {
            Console.WriteLine("Įveskite žodžius: ");
            string input = Console.ReadLine();
            string inputClean = input.Replace(" ", "");
            if (inputClean.Length >= settings.MinInputLength)
            {
                userInput = inputClean.ToLower();
                break;
            }
            else
            {
                Console.WriteLine($"Sakinys turi būti bent {settings.MinInputLength} simbolių ilgio.");
            }
        }

        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7037");

        var response = await client.GetAsync($"/api/anagrams/{userInput}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var anagrams = JsonSerializer.Deserialize<List<string>>(json);

        var anagramCount = 0;
        foreach (var anagram in anagrams)
        { 
            if (anagramCount >= settings.MaxAnagramCount)
                break;
            Console.WriteLine(anagram);
            anagramCount++;
        }

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}