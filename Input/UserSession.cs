using Spelshoppen.MenuPages;
using Spelshoppen.Models;

namespace Spelshoppen;

public class UserSession
{
    #region States
    public MenuState State { get; set; } = MenuState.MainMenu;
    public CheckoutState Status { get; set; } = CheckoutState.ReviewingCart;
    #endregion    
    
    #region CartMenu and Order properties
    //TODO: ADD ADDITIONAL FOR REQUIREMENT
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string PaymentMethodName { get; set; } = string.Empty;
    
    public int SelectedCountryId { get; set; }
    public int SelectedPaymentMethodId { get; set; }
    public string? CityName { get; set; }
    #endregion
    
    #region Category properties
    public int SelectedCategoryId { get; set; }
    public int SelectedProductId { get; set; }
    #endregion
    
    #region Search Properties
    //Sökproperties på produkter via Dapper
    public List<ProductSearchResult> SearchResults { get; set; } = new();
    public string CurrentSearchterm { get; set; }
    #endregion
    
    public Dictionary<ProductItem, int> CartItem { get; set; } = new();
    
    public bool IsRunning { get; set; } = true;
    //Butiksmeddelande
    public string NotificationMessage { get; set; }
    
    //Stegräknare i menyerna
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
    
    //Nollställer valen i menyerna
    public void ResetSelection()
    {
        SelectedCategoryId = 0;
        SelectedProductId = 0;
        
    }
    //Nollställer sökning
    public void ClearSearch()
    {
        CurrentSearchterm = string.Empty;
        SearchResults.Clear();
    }
    
    
    
}