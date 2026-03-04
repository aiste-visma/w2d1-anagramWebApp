
using Microsoft.SemanticKernel;
using System.ComponentModel;

public class RandomNrPlugin
{
    private readonly Random _random = new Random();

    [KernelFunction]
    [Description("Generate a random number within a specified range")]
    public int GenerateRandomNumber(
        [Description("The minimum value (inclusive)")] int min,
        [Description("The maximum value (inclusive)")] int max)
    {
        Console.WriteLine($"[PLUGIN INVOKED] Generating random number between {min} and {max}");
        
        if (min > max)
        {
            throw new ArgumentException($"Minimum value ({min}) cannot be greater than maximum value ({max})");
        }

        int result = _random.Next(min, max + 1);
        Console.WriteLine($"[PLUGIN RESULT] Random number = {result}");
        return result;
    }
}