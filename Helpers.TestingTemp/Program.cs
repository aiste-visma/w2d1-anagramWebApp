using AnagramSolver.BusinessLogic;
public class Person
{
    public string Name { get; set; }
}

class Program
{
        static void Main(string[] args)
    {
        var actions = new Dictionary<string, Func<string, string>>();

        actions.Add("upper", Helpers.upper);
        actions.Add("lower", Helpers.lower);
        actions.Add("reverse", Helpers.reverse);

        var result1 = actions["upper"]("Hello");
        var result2 = actions["lower"]("Hello");
        var result3 = actions["reverse"]("Hello");
        Console.WriteLine($"{result1}, {result2}, {result3}");
    }
}