using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

//using AnagramSolver.EF.DatabaseFirst;
//using AnagramSolver.EF.DatabaseFirst.Models;
using AnagramSolver.EF.CodeFirst.Models;
using AnagramSolver.Dapper;


class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        //AppSettings settings = LoadAppSettings.FromJson("appsettings.json");
        //var zodynas = new WordRepository("zodynas.txt");

        //string userInput;
        //while (true)
        //{
        //    Console.WriteLine("Įveskite žodžius: ");
        //    string input = Console.ReadLine();
        //    string inputClean = input.Replace(" ", "");
        //    if (inputClean.Length >= settings.MinInputLength)
        //    {
        //        userInput = inputClean.ToLower();
        //        break;
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Sakinys turi būti bent {settings.MinInputLength} simbolių ilgio.");
        //    }
        //}

        //using var client = new HttpClient();
        //client.BaseAddress = new Uri("https://localhost:7037");

        //var response = await client.GetAsync($"/api/anagrams/{userInput}");
        //response.EnsureSuccessStatusCode();

        //var json = await response.Content.ReadAsStringAsync();
        //var anagrams = JsonSerializer.Deserialize<List<string>>(json);

        //var countedAnagrams = anagrams.Take(settings.MaxAnagramCount);
        //foreach (var anagram in countedAnagrams)
        //{
        //    Console.WriteLine(anagram);
        //}

        //CancellationToken ct = new CancellationToken();
        //var dictionaryActions = new LINQwithDictionary(zodynas);
        //await dictionaryActions.LINQoperations(ct);

        var context = new AnagramDbContext();
        var database = new EFcoreQuereis(context);

        //database.AddToCategories();
        //database.AddToWords();
        //database.AddNewWord("bekategoris");

        //database.WordsWithCategories();
        //database.FilterByCategory("Veiksmažodis");
        //database.CountByCategory();
        //database.NoCategory();

        string connectionString = "Server=localhost\\SQLEXPRESS;Database=AnagramSolver_CF;Trusted_Connection=True;TrustServerCertificate=True;";
        var repo = new DapperRepo(connectionString);
        repo.GetAllWords();
        //repo.Insert("kurti", 2);
        repo.Update("kurti", "veikti");
        repo.Delete("veikti");
        database.FilterByCategory("Veiksmažodis");

        //var longWords = context.Words.Where(w => w.Value.Length > 6);
        //foreach (var w in longWords)
        //    Console.WriteLine(w.Value);

        //var startsWithS = context.Words.Where(w => w.Value.StartsWith("s"));
        //Console.WriteLine("Words that start with 's'");
        //foreach (var w in startsWithS)
        //    Console.WriteLine(w.Value);

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}
