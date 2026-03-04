using Microsoft.SemanticKernel;
using System.ComponentModel;

public class MathPlugin
{
    [KernelFunction]
    [Description("Calculate the square root of a number")]
    public double SquareRoot([Description("The number to calculate square root for")] double number)
    {
        Console.WriteLine($"[PLUGIN INVOKED] Calculating square root of {number}");
        if (number < 0)
            throw new ArgumentException("Cannot calculate square root of a negative number", nameof(number));
        
        return Math.Sqrt(number);
    }

    [KernelFunction]
    [Description("Add two numbers together")]
    public double Add(
        [Description("The first number")] double a,
        [Description("The second number")] double b)
    {
        Console.WriteLine($"[PLUGIN INVOKED] Adding {a} + {b}");
        double result = a + b;
        Console.WriteLine($"[PLUGIN RESULT] {a} + {b} = {result}");
        return result;
    }

    [KernelFunction]
    [Description("Multiply two numbers together")]
    public double Multiply(
        [Description("The first number")] double a,
        [Description("The second number")] double b)
    {
        Console.WriteLine($"[PLUGIN INVOKED] Multiplying {a} * {b}");
        double result = a * b;
        Console.WriteLine($"[PLUGIN RESULT] {a} * {b} = {result}");
        return result;
    }
}