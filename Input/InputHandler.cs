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
        if (int.TryParse(firstDigit + restOfInput, out int id))
        {
            return id;
        }

        return 0;
    }

    public static int GetAdminIdInput(string message)
    {
        Helpers.UpdateAndSetCursorPosition();
        Console.Write(message);
        char firstChar = Console.ReadKey(true).KeyChar;
        return PromptForId(firstChar);
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