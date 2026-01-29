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
        Console.WriteLine($"Žodžiai turi būti bent {settings.MinInputLength} raidžių ilgio.");
    }
}


var solver = new MultipleAnagramFinder(zodynas);
var anagrams = solver.GetAnagrams(userInput);


int anagramCount = 0;

foreach (var anagram in anagrams.Reverse())
{
    Console.WriteLine(anagram);
    if (anagramCount == settings.MaxAnagramCount)
        break;
    anagramCount++;
}

