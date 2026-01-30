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
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string StreetName { get; set; } = "";
    public string CountryName { get; set; } = "";
    public string PaymentMethodName { get; set; } = "";
    
    public int SelectedCountryId { get; set; }
    public int SelectedPaymentMethodId { get; set; }
    public string? CityName { get; set; }
    #endregion
    
    #region Category properties
    public int SelectedCategoryId { get; set; }
    public int SelectedProductId { get; set; }
    #endregion
    
    #region Search Properties

    public List<ProductSearchResult> SearchResults { get; set; } = new();
    public string CurrentSearchterm { get; set; }
    #endregion
    public Dictionary<ProductItem, int> CartItem { get; set; } = new();
    
    public bool IsRunning { get; set; } = true;
    public string NotificationMessage { get; set; }
    
    
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

    public void ClearSearch()
    {
        CurrentSearchterm = "";
        SearchResults.Clear();
    }
    public void ResetAfterPurchase()
    {
        CartItem.Clear();
        FirstName = "";
        LastName = "";
        StreetName = "";
        CityName = "";
        SelectedCountryId = 0;
        SelectedPaymentMethodId = 0;
        SelectedProductId = 0;
        SelectedCategoryId = 0;
        
    }
    
    
}