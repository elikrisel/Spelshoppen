using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class UIRenderer
{
    public static void DrawPage(MenuState state, MyDbContext db, UserSession session, int cartCount)
    {
        Console.Clear();
        Lowest.LowestPosition = 0;
        
        GlobalPage.GlobalLayout(state);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(80, 2);
        Console.WriteLine($"Varukorg: {cartCount} stycken");
        Console.ResetColor();

        switch (state)
        {
            case MenuState.MainMenu:
                GlobalPage.StartPage();
                break;
            case MenuState.CategoryMenu:
                CategoryMenu.Draw(db,session);
                break;
            case MenuState.AdminMenu:
                break;
            case MenuState.CartMenu:
                break;
            case MenuState.Quit:
                break;
            
        }
        
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

        
        session.NotificationMessage = null;
    }

    public static void DrawPrompts(UserSession session)
    {
        string[] prompts = 
        {
            "Skriv Kategori-ID",
            "Skriv Produkt-ID för att visa information",
            "Tryck [ENTER] för att Köpa eller skriv ett annat ID för att gå till en annan produkt:"
        };
        
        // Om det fanns en notis kommer LowestPosition ha ökat
        Console.SetCursorPosition(0, Console.CursorTop + 1); 
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" >> {prompts[session.CurrentStep]}: ");
        Console.ResetColor();
    }
    
    
    
}