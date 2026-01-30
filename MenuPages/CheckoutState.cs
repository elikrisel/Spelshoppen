namespace Spelshoppen.MenuPages;

//Stegprocess vid betalning
public enum CheckoutState
{
    ReviewingCart,
    EditingItem,
    EnteringDetails,
    ProcessOrder
}