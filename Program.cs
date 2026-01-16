using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen;

//Shoppen och kundkorgen högsta prioritet
class Program
{
    static void Main(string[] args)
    {
        bool isRunning = true;
        string lastAction = "Startsida";
        
        
        while (isRunning)
        { 
            Console.Clear();
            Lowest.LowestPosition = 0;
            
        var windowTop = new UX.Window("", 45, 1, new List<string> { "# Spelshoppen #", "Finns nu i Konsol app!" });
        windowTop.Draw();
        
        var windowMenu = new UX.Window("Kundmeny", 2, 1, new List<string> { "1. Startsida", "2. Shoppen", "3. Varukorgen" });
        windowMenu.Draw();
        
        var windowCart = new UX.Window("Din varukorg", 75, 1, new List<string> { "1 st PS4, Metro Exodus", "1 st NSW Pro Controller", "Tryck X för att checka ut" });
        windowCart.Draw();
        
        var windowBestSellers = new UX.Window("Bäst säljande produkter", 25, 6, new List<string> { 
            "NSW2, The Legend of Zelda: Tears of the Kingdom",
            "PS5, The Last Of Us Part 2: Remastered", "XBONE, Starfield" 
        });
        windowBestSellers.Draw();
        
        var windowCategories = new UX.Window("Kategorier", 2, 20, new List<string> { "1. Spel", "2. Konsoler", "3. Tillbehör" });
        windowCategories.Draw();
        
        WindowExample.DrawShop();
        
        var windowAdmin = new UX.Window("Admin", 75, 20, new List<string> { "4. Produkter", "5. Kategorier", "6. Kunder", "7. Statistik" });
        windowAdmin.Draw();
        
        var windowStatus = new UX.Window("Systemstatus", 35, 20, new List<string> { lastAction });
        windowStatus.Draw();

        Console.SetCursorPosition(0, Lowest.LowestPosition + 1);
        Console.WriteLine("Navigera genom att trycka på knapparna i fönstren [Tryck Q för att avsluta]");
        
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        
        switch (char.ToUpper(keyInfo.KeyChar))
        {
            case '1':
                lastAction = "Laddar Startsidan...";
                break;
            case '2':
                lastAction = "Öppnar Shoppen...";
                break;
            case 'A':
                lastAction = "La till 'Tröja' i varukorgen!";
                break;
            case 'B':
                lastAction = "La till 'Byxor' i varukorgen!";
                break;
            case 'C':
                lastAction = "La till 'Läderskor' i varukorgen!";
                break;
            case 'X':
                lastAction = "Går till kassan...";
                break;
            case '4':
                lastAction = "Öppnar Admin: Produkter";
                break;
            case 'Q':
                lastAction = "Avslutar Shoppen";
                isRunning = !isRunning;
                break;
            default:
                lastAction = $"Knapp '{keyInfo.KeyChar}' har ingen funktion än.";
                break;
        }
            
            
         #region Testing with local database   
        //     using (var db = new MyDbContext())
        //     {
        //         var categories = new List<Category>
        //         {
        //             new Category { Title = "Spel ", Description = "Vår spelkollektion till alla plattformar!" },
        //             new Category { Title = "Konsoler", Description = "Hårdvara" },
        //             new Category { Title = "Tillbehör", Description = "Handkontroller, kablar etc." }
        //         };
        //         db.AddRange(categories);
        //         db.SaveChanges();
        //
        //         var genres = new List<Genre>
        //         {
        //             new Genre {Name = "Action"},
        //             new Genre {Name = "FPS"},
        //             new Genre {Name = "RPG"},
        //             new Genre {Name = "Platformer"},
        //             new Genre {Name = "Horror"}
        //             
        //         };
        //         db.AddRange(genres);
        //         db.SaveChanges();
        //         var supplier = new List<Supplier>
        //         {
        //             new Supplier
        //                 { PublisherName = "Sony", ContactName = "Hiraka Yoshida", ContactInformation = "hiyo@sony.jp" },
        //             new Supplier
        //             {
        //                 PublisherName = "Microsoft", ContactName = "Filip Spenderare",
        //                 ContactInformation = "fisp@ms.net"
        //             },
        //             new Supplier
        //             {
        //                 PublisherName = "Nintendo", ContactName = "Göran Folkskog",
        //                 ContactInformation = "gofo@nintendo.se"
        //             }
        //         };
        //         db.AddRange(supplier);
        //         db.SaveChanges();
        //         var eldenRing = new Product
        //         {
        //             Title = "Elden Ring",
        //             Description = "Ett episkt äventyr med action element!",
        //             CategoryId = categories[0].Id,
        //         };
        //         db.Products.Add(eldenRing);
        //         db.SaveChanges();
        //         
        //         db.ProductGenres.AddRange(
        //             new ProductGenre { ProductId = eldenRing.Id, GenreId = genres[0].Id },
        //             new ProductGenre { ProductId = eldenRing.Id, GenreId = genres[2].Id }
        //         );
        //         db.ProductItems.Add(new ProductItem
        //         {
        //             ProductId = eldenRing.Id,
        //             SupplierId = supplier[0].Id,
        //             Price = 599.99m,
        //             UnitsInStock = 7,
        //             Condition = "Nytt",
        //             IsFeatured = true
        //         });
        //         db.SaveChanges();
        //
        //     }
        //     
        #endregion
        }
        
    }
}
