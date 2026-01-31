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
        
        new UX.Window("VÄLKOMMEN!", 40, 6, new List<string> { 
            "Välkommen till Spelshoppen v1.0!",
            "Använd menyknapparna till vänster för att gå igenom vårt sortiment!."
        }).Draw();

        var featured = StoreServices.GetFeaturedItems(db);
        
        //Bestämda positioner i arrayer som jag loopar igenom för att sätta x och y positionen på alla tre erbjudanden
        int[] xPosition = Helpers.SetXPositionOnFeatured;
        int yPosition = Helpers.SetYPositionOnFeatured;
        
        
        for (int i = 0; i < featured.Count; i++)
        {
            var item = featured[i];
            string titleWithSpacing = item.Products.Title.PadRight(Helpers.MaxTitleLengthOnFeatured);
            new UX.Window($"ERBJUDANDE {i + 1}", xPosition[i], yPosition, 
                new List<string> { 
                titleWithSpacing, 
                $"{item.Price} kr", 
                $"[{i + 1}] KÖP NU" 
            }).Draw();
            
        }
        UIRenderer.DrawNotifications(session);
        
    }
    
    
    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        //Hämtar databasen
        var featured = StoreServices.GetFeaturedItems(db);

        switch (input)
        {
            case '1':
                if(featured.Count >= 1) //Säkerhetskontroller 
                    StoreServices.ExecutePurchase(db,session,featured[0]);
                break;
            case '2':
                if(featured.Count >= 2)
                    StoreServices.ExecutePurchase(db,session,featured[1]);
                break;
            case '3':
                if(featured.Count >= 3)
                    StoreServices.ExecutePurchase(db,session,featured[2]);
                break;
            default:
                Console.WriteLine($"Du kan bara mellan tre erbjudanden just nu!");
                break;
        }
        
        
    }
    
}