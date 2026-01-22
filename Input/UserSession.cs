using Spelshoppen.Models;

namespace Spelshoppen;

public class UserSession
{
    public MenuState State { get; set; } = MenuState.MainMenu;
    public int SelectedCategoryId { get; set; }
    public int SelectedProductId { get; set; }
    
    public List<ProductItem> Cart { get; set; } = new();

    public void ResetSelection()
    {
        SelectedCategoryId = 0;
        SelectedProductId = 0;
    }
}