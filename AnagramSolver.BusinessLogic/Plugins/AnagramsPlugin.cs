using AnagramSolver.Contracts;
using Microsoft.SemanticKernel;
using System.ComponentModel;

public class AnagramPlugin
{
    private readonly IAnagramSolver _anagramSolver;


    public AnagramPlugin(IAnagramSolver anagramSolver)
    {
        _anagramSolver = anagramSolver;
        
    }

    [KernelFunction]
    [Description("Find all anagrams of a given word from the dictionary")]
    public async Task<string[]> FindAnagrams([Description("The word to find anagrams for")] string word)
    {
        Console.WriteLine($"[PLUGIN INVOKED] Finding anagrams of '{word}'");
        
        if (string.IsNullOrWhiteSpace(word))
            return Array.Empty<string>();

        try
        {
            var anagrams = await _anagramSolver.GetAnagramsAsync(
                word, 
                (msg) => { },
                CancellationToken.None
            );

            var singleWordAnagrams = anagrams
                .Where(a => !a.Contains(' '))
                .ToArray();

            Console.WriteLine($"[PLUGIN RESULT] Found {singleWordAnagrams.Length} anagrams");
            return singleWordAnagrams;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to find anagrams: {ex.Message}");
            return Array.Empty<string>();
        }
    }

    [KernelFunction]
    [Description("Count how many anagrams exist for a given word")]
    public async Task<int> CountMatches([Description("The word to count anagrams for")] string word)
    {
        Console.WriteLine($"[PLUGIN INVOKED] Counting anagrams of '{word}'");
        
        if (string.IsNullOrWhiteSpace(word))
            return 0;

        try
        {
            var anagrams = await _anagramSolver.GetAnagramsAsync(
                word, 
                (msg) => { }, 
                CancellationToken.None
            );
            
            int count = anagrams.Count(a => !a.Contains(' '));

            Console.WriteLine($"[PLUGIN RESULT] Count = {count}");
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to count anagrams: {ex.Message}");
            return 0;
        }
    }
}