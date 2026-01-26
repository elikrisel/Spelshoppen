using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class UIRenderer
{
    public static void DrawBaseLayout(UserSession session)
    {
        Console.Clear();
        UX.Lowest.LowestPosition = 0;
        
        //Debug Window:
        Console.ForegroundColor = ConsoleColor.Red;
        Helpers.ShowDebugInfo(session.State);
        // Varukorg
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(80, 2);
        Console.Write($"Varukorg: {session.CartItem.Count} stycken");
        Console.ResetColor();

        // Toppfönster
        new UX.Window("", 45, 1, new List<string> { "# Spelshoppen #", "Finns nu i Konsol app!" }).Draw();
    
        // Sidomeny
        var menuRows = InputHandler.MenuLabel
            .Where(kvp => kvp.Key != session.State)
            .Select(kvp => kvp.Value).ToList();
        new UX.Window("Kundmeny", 2, 1, menuRows).Draw();
    }

    public static void DrawNotifications(UserSession session)
    { 
        if (string.IsNullOrEmpty(session.NotificationMessage)) return;

        Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(Helpers.ShowXNumberOfLines(60));
        Console.WriteLine($" NOTIS: {session.NotificationMessage}");
        Console.WriteLine(Helpers.ShowXNumberOfLines(60));
        Console.ResetColor();

    }
    
    //TODO: Flytta över den här till varje meny
    public static void DrawCategoryPrompts(UserSession session)
    {
        string[] prompts = 
        {
            "Skriv Kategori-ID",
            "Skriv Produkt-ID för att visa information",
            "Tryck [ENTER] för att Köpa eller skriv ett annat ID för att gå till en annan produkt:"
        };
        
        Console.SetCursorPosition(0, Console.CursorTop + 1); 
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" >> {prompts[session.CurrentStep]}: ");
        Console.ResetColor();
    }
    
    
    
}