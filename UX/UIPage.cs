namespace Spelshoppen.UX;

public class UIPage
{
    public static Dictionary<MenuState, string> Labels = new()
    {
        { MenuState.MainMenu,      "[S] Startsida" },
        { MenuState.CategoryMenu,  "[K] Kategorier"},
        {MenuState.AdminMenu,      "[A] Admin"     },
        { MenuState.CartMenu,      "[V] Varukorgen"},
        { MenuState.Quit,          "[Q] Quit"      }
    };

    public static Dictionary<char, MenuState> KeyBindings = new()
    {
        {'S', MenuState.MainMenu},
        {'K', MenuState.CategoryMenu},
        {'A', MenuState.AdminMenu},
        {'C', MenuState.CartMenu},
        {'Q', MenuState.Quit},
    };

    public static void GlobalLayout(MenuState state)
    {
        var windowTop = new UX.Window("", 45, 1, new List<string> { "# Spelshoppen #", "Finns nu i Konsol app!" });
        windowTop.Draw();
        
        var menuRows = Labels.Where(kvp => kvp.Key != state)
            .Select(kvp => kvp.Value).ToList();

        var windowMenu = new UX.Window("Kundmeny", 2, 1, menuRows);
        windowMenu.Draw();
        //var windowMenu = new UX.Window("Kundmeny", 2, 1, new List<string> { "[S] Startsida", "[K] Kategorier", "[V] Varukorgen", "[A] Avsluta" });
    }
    
    public static void StartPage()
    {
        var welcomeWindow = new UX.Window("Välkommen!", 25, 6, new List<string> { "Välkommen till Spelshoppen!" });
        welcomeWindow.Draw();
        
    }
}