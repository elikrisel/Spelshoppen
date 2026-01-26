using Spelshoppen.MenuPages;
using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen;

public class InputHandler
{
    //ID Input, tillåter dig att skriva mer än ensiffrigt
    public static int PromptForId(char firstDigit)
    {
        Console.Write(firstDigit);
        string? rest = Console.ReadLine();
        return int.TryParse(firstDigit + rest, out int id) ? id : 0;
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
        { MenuState.MainMenu, "[S] Startsida" },
        { MenuState.CategoryMenu, "[K] Kategorier" },
        { MenuState.AdminMenu, "[A] Admin" },
        { MenuState.CartMenu, "[C] Varukorgen" },
        { MenuState.Quit, "[Q] Quit" }
    };
    
    //Keybindings 
    private static readonly Dictionary<char, MenuState> KeyBindings = new()
    {
        { 'S', MenuState.MainMenu },
        { 'K', MenuState.CategoryMenu },
        { 'A', MenuState.AdminMenu },
        { 'C', MenuState.CartMenu },
        { 'Q', MenuState.Quit },
    };
    
    
    public static readonly Dictionary<MenuState, IMenuPage> Pages = new()
    {
        { MenuState.MainMenu, new MainMenu() },
        { MenuState.CategoryMenu, new CategoryMenu() },
        { MenuState.CartMenu, new CartMenu() },
        { MenuState.AdminMenu, new AdminMenu() },
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
            while (Console.KeyAvailable) Console.ReadKey(true);
            return;
        }

        //Skickar oss till nästa sida i state
        if (Pages.TryGetValue(session.State, out var currentPage))
        {
            currentPage.HandleInput(key, input, db, session);
            if (session.State == MenuState.MainMenu && key.Key == ConsoleKey.Enter)
            {
                while (Console.KeyAvailable) Console.ReadKey(true);
            }
        }
    }
}