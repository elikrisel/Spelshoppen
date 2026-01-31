using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class UIRenderer
{
    public static void DrawBaseLayout(UserSession session)
    {
        Console.Clear();
        #region Debug
        //Debug Window:
        //Helpers.ShowDebugInfo(session.State);
        // Varukorg
        Helpers.ShowCart(session);
        //Trackar Kategori val
        //Helpers.ShowDebugInCategorySelection(session);
        #endregion
        
        // Toppfönster
        new UX.Window("", 45, 1, new List<string> { "# Spelshoppen #", "Finns nu i Konsol app!" }).Draw();
    
        // Sidomeny som skrivs ut, den döljer menulabel beroende på vilken sida man är inne på, i.e.
        //kategori texten kommer inte synas när man är inne i kategorier
        var menuRows = InputHandler.MenuLabel
            .Where(kvp => kvp.Key != session.State)
            .Select(kvp => kvp.Value).ToList();
        new UX.Window("Kundmeny", 2, 1, menuRows).Draw();
    }
    //Ritar ut Kategori menyn
    public static void DrawCategoryMenu(MyDbContext db,string menuName)
    {
        var categories = db.Categories.Select(c => $"[{c.Id}] {c.Title}").ToList();
        new UX.Window( $"{menuName}", 10, 8, categories).Draw();
    }
    //Ritar ut produkten som är vald
    public static void DrawProductWindow(MyDbContext db, int categoryId)
    {
        var products = db.Products.Where(p => p.CategoryId == categoryId)
            .Select(p => $"[{p.Id}] {p.Title}").ToList();
        new UX.Window("PRODUKTER", 30, 8, products).Draw();
    }
    //Ritar prompts beroende på vilket steg man befinner sig i
    public static void DrawCategoryPrompts(UserSession session)
    {
        string[] prompts = 
        {
            "Skriv Kategori-ID",
            "Skriv Produkt-ID för att visa information",
            "Tryck [ENTER] för att Köpa eller skriv ett annat ID för att gå till en annan produkt"
        };
    
        // Bestämmer en fast rad, 5 rader från botten av konsolen
        int fixedRow = Console.WindowHeight - 5; 

        //Går till raden under och tar bort
        Helpers.ClearLine(fixedRow);
        
        Helpers.ClearLine(fixedRow + 1);
        
        Console.SetCursorPosition(0, fixedRow);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" >> {prompts[session.CurrentStep]}: ");
        Console.ResetColor();
    }
    //Kollar så den skriver ut notiser vid interaktioner i shoppen
    public static void DrawNotifications(UserSession session)
    {
        int notificationRow = Console.WindowHeight - 7;
        if (!string.IsNullOrEmpty(session.NotificationMessage))
        {
            

            // Rensar raden först om gamla notiser ligger kvar
            Helpers.ClearLine(notificationRow);

            //Skriver ut den nya notisen
            Console.SetCursorPosition(2, notificationRow);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"! {session.NotificationMessage} ");
            Console.ResetColor();
            
            //Rensar meddelande
            session.NotificationMessage = "";
        }
    }
    
    
}