using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class UIRenderer
{
    public static void Display(MenuState state, MyDbContext db, int categoryId, int prodId, int cartCount)
    {
        Console.Clear();
        Lowest.LowestPosition = 0;
        
        UIPage.GlobalLayout(state);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(80, 2);
        Console.WriteLine($"Varukorg: {cartCount} stycken");
        Console.ResetColor();

        switch (state)
        {
            case MenuState.MainMenu:
                UIPage.StartPage();
                break;
            case MenuState.CategoryMenu:
                CategoryMenu.DrawCategoryMenu(db,categoryId,prodId);
                break;
            case MenuState.AdminMenu:
                break;
            case MenuState.CartMenu:
                break;
            case MenuState.Quit:
                break;
            
        }
        
    }
}