using Spelshoppen.UX;

namespace Spelshoppen;

//TODO: Flytta mer saker till Helpers
public class Helpers
{
    public static void ShowDebugInfo(MenuState state) => Console.Write($"Nuvarande State: {state}");
    public static string ShowXNumberOfLines(int number) => new('-', number);

    public static void UpdateAndSetCursorPosition()
    {
        int newCursorPosition = Lowest.LowestPosition + 2;
        int rowCount = 5;
        Console.SetCursorPosition(0, newCursorPosition);

        for (int i = 0; i < rowCount; i++)
        {
            Console.WriteLine(new string(' ',Console.WindowWidth));
        }
        Console.SetCursorPosition(0, newCursorPosition);
        
    }
    
}