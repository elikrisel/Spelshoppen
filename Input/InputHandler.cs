using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen;

public class InputHandler
{
    // public static int PromptForId(char firstDigit)
    // {
    //     int newTopPosition = Lowest.LowestPosition + 2;
    //     Console.SetCursorPosition(0, newTopPosition);
    //     
    //     Console.Write(new string(' ', Console.WindowWidth));
    //     Console.SetCursorPosition(0, newTopPosition);
    //
    //     Console.Write($" Skriv ID [BÖRJA MED {firstDigit}]: {firstDigit}");
    //     
    //     string? restOfInput = Console.ReadLine();
    //     return int.TryParse(firstDigit + restOfInput, out int id) ? id : 0;
    // }
    
    
    
    public static int PromptForId(char firstDigit)
    {
        Console.Write(firstDigit); // Skriver ut första siffran direkt efter "Skriv ID: "
        string? restOfInput = Console.ReadLine();
    
        if (int.TryParse(firstDigit + restOfInput, out int id))
            return id;
    
        return 0;
    }
    
    public static readonly Dictionary<MenuState, string> Labels = new()
    {
        { MenuState.MainMenu,      "[S] Startsida" },
        { MenuState.CategoryMenu,  "[K] Kategorier"},
        {MenuState.AdminMenu,      "[A] Admin"     },
        { MenuState.CartMenu,      "[V] Varukorgen"},
        { MenuState.Quit,          "[Q] Quit"      }
    };

    public static readonly Dictionary<char, MenuState> KeyBindings = new()
    {
        {'S', MenuState.MainMenu},
        {'K', MenuState.CategoryMenu},
        {'A', MenuState.AdminMenu},
        {'C', MenuState.CartMenu},
        {'Q', MenuState.Quit},
    };
    
    public static void HandleInput(ConsoleKeyInfo key, UserSession session, MyDbContext db)
    {
        char input = char.ToUpper(key.KeyChar);
        
        if (KeyBindings.TryGetValue(input, out var newState))
        {
            session.State = newState;
            session.ResetSelection();
            return;
        }
        
        if (char.IsDigit(input) && session.State == MenuState.CategoryMenu)
        {
            int id = PromptForId(input); 

            if (session.SelectedCategoryId == 0)
            {
                if (db.Categories.Any(c => c.Id == id)) 
                    session.SelectedCategoryId = id;
                else 
                    session.NotificationMessage = $"Kategori {id} finns inte!";
            }
            else 
            {
                if (db.Products.Any(p => p.Id == id && p.CategoryId == session.SelectedCategoryId))
                    session.SelectedProductId = id;
                else 
                    session.NotificationMessage = $"Produkt {id} finns inte i denna kategori!";
            }
            return;
        }

        
        if (key.Key == ConsoleKey.Enter && session.SelectedProductId != 0)
        {
            var item = StoreServices.GetPurchaseableItem(db, session.SelectedProductId);
            if (item != null)
            {
                session.Cart.Add(item);
                session.NotificationMessage = $"{item.Products?.Title} tillagd!";
                session.SelectedProductId = 0;
            }
        }
    }
    
    
    // public static void HandleInput(ConsoleKeyInfo key, UserSession session, MyDbContext db)
    // {
    //     char input = char.ToUpper(key.KeyChar);
    //
    //     // 1. Global Navigering
    //     if (KeyBindings.TryGetValue(input, out var newState))
    //     {
    //         session.State = newState;
    //         session.ResetSelection();
    //         return;
    //     }
    //
    //     // 2. Köp-logik (Enter)
    //     if (key.Key == ConsoleKey.Enter && session.SelectedProductId != 0)
    //     {
    //         var item = StoreServices.GetPurchaseableItem(db, session.SelectedProductId);
    //         
    //     
    //         // Viktigt: Kolla om vi redan har "paxat" alla tillgängliga exemplar i vår Cart
    //         //int alreadyInCart = session.Cart.Count(i => i.Id == item?.Id);
    //
    //         if (item != null)
    //         {
    //             session.Cart.Add(item);
    //             session.SelectedProductId = 0;
    //             UIPage.ShowNotification($"{item.Products?.Title} tillagd i korgen!");
    //         }
    //         else
    //         {
    //             UIPage.ShowNotification("Tyvärr, inte tillräckligt många i lager.");
    //         }
    //         return;
    //     }
    //
    //     // 3. ID-val (Siffror)
    // //     if (char.IsDigit(input) && session.State == MenuState.CategoryMenu)
    // //     {
    // //         int id = PromptForId(input);
    // //     
    // //         // Om vi inte valt kategori än -> Försök välja en
    // //         if (session.SelectedCategoryId == 0)
    // //         {
    // //             if (db.Categories.Any(c => c.Id == id)) 
    // //                 session.SelectedCategoryId = id;
    // //             else 
    // //                 UIPage.ShowNotification("Kategorin finns inte.");
    // //         }
    // //         
    // //         else if (session.SelectedProductId == 0)
    // //         {
    // //             if (db.Products.Any(p => p.Id == id && p.CategoryId == session.SelectedCategoryId)) 
    // //                 session.SelectedProductId = id;
    // //             else 
    // //                 UIPage.ShowNotification("Produkten finns inte i denna kategori.");
    // //         }
    // //     }
    // if (char.IsDigit(input) && session.State == MenuState.CategoryMenu)
    // {
    //     int id = PromptForId(input);
    //
    //     // Om vi inte har valt kategori än
    //     if (session.SelectedCategoryId == 0)
    //     {
    //         if (db.Categories.Any(c => c.Id == id)) 
    //             session.SelectedCategoryId = id;
    //     }
    //     else 
    //     {
    //         // Vi är inne i en kategori. Om vi trycker på ett ID här, 
    //         // så sätter vi SelectedProductId oavsett om vi redan tittade på en annan produkt.
    //         // Detta gör att vi kan "byta" produkt sömlöst.
    //         if (db.Products.Any(p => p.Id == id && p.CategoryId == session.SelectedCategoryId))
    //         {
    //             session.SelectedProductId = id;
    //         }
    //     }
    // }
    //
    // }
    
    
    
}