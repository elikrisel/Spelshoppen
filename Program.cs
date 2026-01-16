using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen;

//Shoppen och kundkorgen högsta prioritet
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            #region Menu - Commented for now
            // //Titel
            // List<string> topText = new List<string> { "# Spelshoppen #", "Finns nu i Konsol app!" };
            // var windowTop = new UX.Window("", 45, 1, topText);
            // windowTop.Draw();
            //
            // // Hämtar från databasen
            // List<string> categoriesText = new List<string> { "1. Spel", "2. Konsoler", "3. Tillbehör" };
            //
            // // Detta hämtas från databas
            // List<string> cartText = new List<string> { "1 st PS4, Metro Exodus", "1 st NSW Pro Controller", "Tryck X för att checka ut" };
            // var windowCart = new UX.Window("Din varukorg", 75, 1, cartText);
            // windowCart.Draw();
            //
            // List<string> topText3 = new List<string> { "1. Startsida", "2. Shoppen", "3. Varukorgen" };
            // var windowTop3 = new UX.Window("Kundmeny", 2, 1, topText3);
            // windowTop3.Draw();
            //
            //
            // var windowCategories = new UX.Window("Kategorier", 2, 20, categoriesText);
            // windowCategories.Draw();
            // WindowExample.DrawShop();
            //
            //
            //
            // List<string> topText2 = new List<string> { "NSW2, The Legend of Zelda: Tears of the Kingdom",
            //     "PS5, The Last Of Us Part 2: Remastered", "XBONE, Starfield" };
            // var windowTop2 = new UX.Window("Bäst säljande produkter", 25, 6, topText2);
            // windowTop2.Draw();
            //
            //
            // List<string> topText4 = new List<string> { "1. Administrera produkter", "2. Administrera kategorier", "3. Administrera kunder", "4. Se statistik(Queries)" };
            // var windowTop4 = new UX.Window("Admin", 75, 20, topText4);
            // windowTop4.Draw();
            //
            //
            //
            // Console.WriteLine();
            // Console.WriteLine("Tryck för att navigera i menyn");
            //
            // ConsoleKeyInfo keyInfo = Console.ReadKey();
            // Console.Clear();
            //
            //
            // switch (keyInfo.KeyChar)
            // {
            //     
            //     case '1':
            //         Console.WriteLine(" Du har tryckt 1");
            //         break;
            //     
            // }
            #endregion

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
        }
        
    }
}
