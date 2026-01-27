using Spelshoppen.UX;

namespace Spelshoppen;

//TODO: Flytta mer saker till Helpers
public class Helpers
{
    //Kollar vilket state jag är i under programmets gång
    public static void ShowDebugInfo(MenuState state)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"Nuvarande State: {state}");
        Console.ResetColor();
    }

    public static void ShowDebugInCategorySelection(UserSession session)
    {
        Console.SetCursorPosition(0, 28);
        Console.Write($"Nuvarande selection. CategoryId: {session.SelectedCategoryId} | ProductId: {session.SelectedProductId}");
    }

    public static void ShowCart(UserSession session)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(80, 2);
        Console.Write($"Varukorg: {session.CartItem.Count} stycken");
        Console.ResetColor();
    }
    
    //Printar x linjer enligt användaren
    public static string PrintXNumberOfLines(int number) => new('-', number);

    //Sätter ny cursor position och flyttar ner ytterligare rader
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
    public static string Prompt(string message)
    {
        Console.CursorVisible = true;
        Console.Write(message);
        string input = Console.ReadLine() ?? "";
        Console.CursorVisible = false;
        if (string.IsNullOrWhiteSpace(input)) throw new Exception("Tom inmatning");
        return input;
    }
    
}