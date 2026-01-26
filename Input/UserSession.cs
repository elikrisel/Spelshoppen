using Spelshoppen.MenuPages;
using Spelshoppen.Models;

namespace Spelshoppen;

public class UserSession
{
    public MenuState State { get; set; } = MenuState.MainMenu;
    public CheckoutState Status { get; set; } = CheckoutState.ReviewingCart;

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string StreetName { get; set; } = "";
    public string CountryName { get; set; } = "";
    public string PaymentMethodName { get; set; } = "";
    
    public int SelectedCountryId { get; set; }
    
    public int SelectedPaymentMethodId { get; set; }
    public int SelectedCategoryId { get; set; }
    public int SelectedProductId { get; set; }
    public string NotificationMessage { get; set; }
    public string? CityName { get; set; }
    public Dictionary<ProductItem, int> CartItem { get; set; } = new();
    
    //Steptracker för CategoryMenu 
    //TODO: STEPTRACKER IN CARTMENU?
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