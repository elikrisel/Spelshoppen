using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

public class MainMenu : IMenuPage
{
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        
        UIRenderer.DrawBaseLayout(session);
        
        new UX.Window("Välkommen!", 15, 8, new List<string> { 
            "Välkommen till Spelshoppen v1.0!",
            "Använd menyknapparna till vänster för att gå igenom vårt sortiment!."
        }).Draw();
        
        var featured = db.ProductItems.Include(pi => pi.Products)
            .Where(pi => pi.IsFeatured && pi.UnitsInStock > 0).Take(3).ToList();

        //Hårdkodar positionen som den ska flytta på tills vidare
        //TODO: Refakturera

            new UX.Window("ERBJUDANDE 1", 15, 15, new List<string> { 
                featured[0].Products.Title, 
                $"{featured[0].Price} kr, [{featured[0].UnitsInStock}] kvar!", 
                $"{featured[0].Condition}!",
                $"Tryck 1 för att köpa"
            }).Draw();

        
            new UX.Window("ERBJUDANDE 2", 42, 15, new List<string> { 
                featured[1].Products.Title, 
                $"{featured[1].Price} kr,[{featured[1].UnitsInStock}] kvar!",
                $"{featured[1].Condition}!",
                $"Tryck 2 för att köpa"
            }).Draw();

        
            new UX.Window("ERBJUDANDE 3", 69, 15, new List<string> { 
                featured[2].Products.Title, 
                $"{featured[2].Price} kr,[{featured[2].UnitsInStock}] kvar!",
                $"{featured[2].Condition}!",
                $"Tryck 3 för att köpa"
            }).Draw();    
        
        

        
        UIRenderer.DrawNotifications(session);
        
    }
    
    //TODO: Fylla i sen
    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        //Hämtar databasen
        var featured = db.ProductItems.Include(pi => pi.Products)
            .Where(pi => pi.IsFeatured && pi.UnitsInStock > 0).Take(3).ToList();   
        
        switch (input)
        {
            case '1':
                if (featured.Count >= 1) ExecutePurchase(db, session,featured[0]);
                break;
            case '2':
                if (featured.Count >= 2) ExecutePurchase(db, session,featured[1]);
                break;
            case '3':
                if (featured.Count >= 3) ExecutePurchase(db, session,featured[2]);
                break;
        }
        
    }
    
    //TODO: REFACTOR SO I CAN USE THIS FOR BOTH MAIN AND CATEGORY MENU
    private void ExecutePurchase(MyDbContext db, UserSession session, ProductItem item)
    {

        if (item != null && item.UnitsInStock > 0)
        {
            item.UnitsInStock--;
            var existingKey = session.CartItem.Keys.FirstOrDefault(k => k.Id == item.Id);
    
            if (existingKey != null)
            {
                session.CartItem[existingKey]++;
            }
            else
            {
                session.CartItem.Add(item, 1);
            }
            
            db.SaveChanges();
            session.NotificationMessage = $"{item.Products?.Title} tillagd i korgen!";
        }
        else
        {
            session.NotificationMessage = $"Varan tog slut. Välj en annan produkt.";
        }
        
    
    }
    
    
}