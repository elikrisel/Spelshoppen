namespace Spelshoppen;

public class Helpers
{
    public static void ShowDebugInfo(MenuState state)
    {
        Console.Write($"Nuvarande State: {state}");
    }

    public static string ShowXNumberOfLines(int number) => new('-', number);
}