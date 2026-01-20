using Spelshoppen.UX;

namespace Spelshoppen;

public class InputHandler
{
    public static int PromptForId(char firstDigit)
    {
        // Flytta markören till en säker plats under fönstren
        int y = Lowest.LowestPosition + 2;
        Console.SetCursorPosition(0, y);
    
        // Rensa raden först så det ser snyggt ut
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, y);

        Console.Write($">> Skriv ID (börja med {firstDigit}): {firstDigit}");
    
        // Läs resten av input (om det är t.ex. 22 så skriver användaren '2' + Enter)
        string? restOfInput = Console.ReadLine();
        if (int.TryParse(firstDigit + restOfInput, out int id))
        {
            return id;
        }
        return 0;
    }
}