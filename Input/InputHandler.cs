using Spelshoppen.MenuPages;
using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen;

public class InputHandler
{
    //ID Input, I det här fallet har jag möjlighet att skriva tvåsiffrigt för att få tag på 9+
    public static int PromptForId(char firstDigit)
    {
        Console.Write(firstDigit);
        string? restOfInput = Console.ReadLine();
        return int.TryParse(firstDigit + restOfInput, out int id) ? id : 0;
    }
    
    //Används i CategoryMenu och AdminMenu för samma stegprocess
    public static void HandleNavigation(MyDbContext db, UserSession session, char input)
    {
        //Nollställ
        int id = PromptForId(input);
        if (id == 0)
        {
            if (session.SelectedProductId != 0) session.SelectedProductId = 0;
            else if (session.SelectedCategoryId != 0) session.SelectedCategoryId = 0;
            return;
        }
        
        //Väljer Kategori
        if (session.SelectedCategoryId == 0)
        {
            if (db.Categories.Any(c => c.Id == id)) session.SelectedCategoryId = id;
            else session.NotificationMessage = $"Kategori {id} finns inte!";
        }
        // Välj produkt
        else if (session.SelectedProductId == 0)
        {
            if (db.Products.Any(p => p.Id == id && p.CategoryId == session.SelectedCategoryId)) 
                session.SelectedProductId = id;
            else session.NotificationMessage = $"Produkten tillhör inte kategorin!";
        }
    }
    
    //String Labels till Menyn
    public static readonly Dictionary<MenuState, string> MenuLabel = new()
    {
        { MenuState.MainMenu, "[M] Startsida" },
        { MenuState.CategoryMenu, "[K] Kategorier" },
        { MenuState.CartMenu, "[C] Varukorgen" },
        { MenuState.AdminMenu, "[A] Admin" },
        { MenuState.SearchMenu, "[S] Sök"},
        { MenuState.Quit, "[Q] Quit" }
    };
    
    //Keybindings till menyerna
    private static readonly Dictionary<char, MenuState> KeyBindings = new()
    {
        { 'M', MenuState.MainMenu },
        { 'K', MenuState.CategoryMenu },
        { 'C', MenuState.CartMenu },
        { 'A', MenuState.AdminMenu },
        { 'S', MenuState.SearchMenu },
        { 'Q', MenuState.Quit },
    };
    
    
    public static readonly Dictionary<MenuState, IMenuPage> Pages = new()
    {
        { MenuState.MainMenu, new MainMenu() },
        { MenuState.CategoryMenu, new CategoryMenu() },
        { MenuState.CartMenu, new CartMenu() },
        { MenuState.AdminMenu, new AdminMenu() },
        {MenuState.SearchMenu, new SearchMenu() },
        { MenuState.Quit, new QuitMenu() }
    };
    
    public static void HandleInput(ConsoleKeyInfo key, UserSession session, MyDbContext db)
    {
        char input = char.ToUpper(key.KeyChar);
        //Kollar KeyInput för menyn
        if (KeyBindings.TryGetValue(input, out var newState))
        {
            session.State = newState;
            session.ResetSelection();
            return;
        }
        //Skickar oss till nästa sida i state
        if (Pages.TryGetValue(session.State, out var currentPage))
        {
            currentPage.PageInput(key, input, db, session);
        }
    }
}