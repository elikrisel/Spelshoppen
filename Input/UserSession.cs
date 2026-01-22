using Spelshoppen.Models;

namespace Spelshoppen;

public class UserSession
{
    public MenuState State { get; set; } = MenuState.MainMenu;
    public int SelectedCategoryId { get; set; }
    public int SelectedProductId { get; set; }
    
    public string NotificationMessage { get; set; }
    public List<ProductItem> Cart { get; set; } = new();

    public int CurrentStep
    {
        get
        {
            int[] numberOfSteps = {SelectedCategoryId, SelectedProductId};
            int step = 0;
            foreach (var number in numberOfSteps)
            {
                if (number != 0) step++;
            }
            return step;
        }
    }
    

    public void ResetSelection()
    {
        SelectedCategoryId = 0;
        SelectedProductId = 0;
    }
}