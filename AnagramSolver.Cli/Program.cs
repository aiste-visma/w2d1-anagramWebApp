using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using System.Linq;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

AppSettings settings = LoadAppSettings.FromJson("appsettings.json");
var zodynas = new WordRepository("zodynas.txt");

string userInput;
while (true)
{
    Console.WriteLine("Iveskite zodi: ");
    string input = Console.ReadLine();
    string inputClean = input.Replace(" ", "");
    if (inputClean.Length >= settings.MinInputLength)
    {
        userInput = inputClean.ToLower();
        break;
    }
    else
    {
        Console.WriteLine($"Žodis turi būti bent {settings.MinInputLength} raidžių ilgio.");
    }
}

var solver = new AnagramFinder(zodynas, settings.MaxAnagramCount);
var anagrams = solver.GetAnagrams(userInput);


foreach (var anagram in anagrams)
{
    Console.WriteLine(anagram);
}

