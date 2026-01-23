using Spelshoppen.Models;
using Spelshoppen.Transactions;

namespace Spelshoppen;

public class UserSession
{
    public MenuState State { get; set; } = MenuState.MainMenu;
    public int CheckoutStep { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? StreetName { get; set; }
    
    public int SelectedCategoryId { get; set; }
    public int SelectedProductId { get; set; }
    public string NotificationMessage { get; set; }
    public string? CityName { get; set; }
    public List<ProductItem> Cart { get; set; } = new();
    
    //Steptracker 
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
        CheckoutStep = 0;
        
    }
}