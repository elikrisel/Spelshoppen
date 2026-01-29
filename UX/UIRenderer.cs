using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class UIRenderer
{
    public static void DrawBaseLayout(UserSession session)
    {
        Console.Clear();
        
        //Debug Window:
        Helpers.ShowDebugInfo(session.State);
        // Varukorg
        Helpers.ShowCart(session);
        //Trackar Kategori val
        Helpers.ShowDebugInCategorySelection(session);
        
        // Toppfönster
        new UX.Window("", 45, 1, new List<string> { "# Spelshoppen #", "Finns nu i Konsol app!" }).Draw();
    
        // Sidomeny
        var menuRows = InputHandler.MenuLabel
            .Where(kvp => kvp.Key != session.State)
            .Select(kvp => kvp.Value).ToList();
        new UX.Window("Kundmeny", 2, 1, menuRows).Draw();
    }
    public static void DrawCategoryMenu(MyDbContext db,string menuName)
    {
        var categories = db.Categories.Select(c => $"[{c.Id}] {c.Title}").ToList();
        new UX.Window( $"{menuName}", 10, 8, categories).Draw();
    }
    public static void DrawProductWindow(MyDbContext db, int categoryId)
    {
        var products = db.Products.Where(p => p.CategoryId == categoryId)
            .Select(p => $"[{p.Id}] {p.Title}").ToList();
        new UX.Window("PRODUKTER", 30, 8, products).Draw();
    }
    
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
        Console.SetCursorPosition(0, fixedRow);
        Console.Write(new string(' ', Console.WindowWidth)); 
        
        Console.SetCursorPosition(0, fixedRow + 1);
        Console.Write(new string(' ', Console.WindowWidth));
        
        Console.SetCursorPosition(0, fixedRow);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" >> {prompts[session.CurrentStep]}: ");
        Console.ResetColor();
    }
    
    
    
}