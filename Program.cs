using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen;

//Shoppen och kundkorgen högsta prioritet
class Program
{
    static void Main(string[] args)
    {
        bool isRunning = true;
        MenuState menuState = MenuState.MainMenu;
        int selectedCategoryId = 0;
        //string lastAction = "Startsida";
        
        #region Databas commented
        // using (var db = new MyDbContext())
        // {
        //     if (!db.Products.Any())
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
        //             new Genre { Name = "Action" },
        //             new Genre { Name = "FPS" },
        //             new Genre { Name = "RPG" },
        //             new Genre { Name = "Platformer" },
        //             new Genre { Name = "Horror" }
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
        //
        //      var products = new List<Product>
        //      {
        //          //Sony
        //          new Product { 
        //              Title = "God of War Ragnarök", CategoryId = categories[0].Id, 
        //              Description = "Kratos och Atreus reser genom de nio världarna i skuggan av Fimbulvintern.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id }, new ProductGenre { GenreId = genres[2].Id } }, // Action, RPG
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 699, UnitsInStock = 12, Condition = "Ny", IsFeatured = true, SupplierId = supplier[0].Id } } 
        //          },
        //          new Product { 
        //              Title = "The Last of Us Part II", CategoryId = categories[0].Id,
        //              Description = "En känsloladdad resa genom ett post-apokalyptiskt USA präglat av hämnd.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id }, new ProductGenre { GenreId = genres[4].Id } }, // Action, Horror
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 299, UnitsInStock = 0, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[0].Id } } 
        //          },
        //          new Product { 
        //              Title = "Horizon Forbidden West", CategoryId = categories[0].Id,
        //              Description = "Utforska den avlägsna västern och bekämpa enorma maskiner som Aloy.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id }, new ProductGenre { GenreId = genres[2].Id } }, // Action, RPG
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 450, UnitsInStock = 5, Condition = "Ny", IsFeatured = true, SupplierId = supplier[0].Id } } 
        //          },
        //          new Product { 
        //              Title = "Bloodborne", CategoryId = categories[0].Id, 
        //              Description = "Möt dina fasor i den förfallna staden Yharnam i detta utmanande action-RPG.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[2].Id }, new ProductGenre { GenreId = genres[4].Id } }, // RPG, Horror
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 199, UnitsInStock = 3, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[0].Id } } 
        //          },
        //          new Product { 
        //              Title = "Ratchet & Clank: Rift Apart", CategoryId = categories[0].Id,
        //              Description = "Hoppa mellan dimensioner för att stoppa en ond kejsare i detta färgstarka äventyr.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id }, new ProductGenre { GenreId = genres[3].Id } }, // Action, Platformer
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 499, UnitsInStock = 10, Condition = "Ny", IsFeatured = true, SupplierId = supplier[0].Id } } 
        //          },
        //      
        //          //Microsoft
        //          new Product { 
        //              Title = "Halo Infinite", CategoryId = categories[0].Id, 
        //              Description = "Master Chief återvänder för att utforska Zeta Halo och bekämpa The Banished.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id }, new ProductGenre { GenreId = genres[1].Id } }, // Action, FPS
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 499, UnitsInStock = 20, Condition = "Ny", IsFeatured = true, SupplierId = supplier[1].Id } } 
        //          },
        //          new Product { 
        //              Title = "Starfield", CategoryId = categories[0].Id,
        //              Description = "Ett nästa generations rollspel som utspelar sig bland stjärnorna från skaparna av Skyrim.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[2].Id } }, // RPG
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 650, UnitsInStock = 10, Condition = "Ny", IsFeatured = true, SupplierId = supplier[1].Id } } 
        //          },
        //          new Product { 
        //              Title = "Gears 5", CategoryId = categories[0].Id, 
        //              Description = "Världen faller samman och Kait Diaz ger sig ut för att avslöja sitt ursprung.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id }, new ProductGenre { GenreId = genres[1].Id } }, // Action, FPS
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 150, UnitsInStock = 2, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[1].Id } } 
        //          },
        //          new Product { 
        //              Title = "State of Decay 2", CategoryId = categories[0].Id, 
        //              Description = "Bygg en bas, hantera resurser och överlev zombieapokalypsen med dina vänner.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[2].Id }, new ProductGenre { GenreId = genres[4].Id } }, // RPG, Horror
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 250, UnitsInStock = 0, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[1].Id } } 
        //          },
        //          new Product { 
        //              Title = "Ori and the Will of the Wisps", CategoryId = categories[0].Id, 
        //              Description = "En vacker och utmanande plattformsupplevelse i en handmålad värld.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[3].Id } }, // Platformer
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 299, UnitsInStock = 15, Condition = "Ny", IsFeatured = false, SupplierId = supplier[1].Id } } 
        //          },
        //      
        //          //Nintendo 
        //          new Product { 
        //              Title = "Zelda: Tears of the Kingdom", CategoryId = categories[0].Id, 
        //              Description = "Ett episkt äventyr på land och i skyarna över det vidsträckta Hyrule.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id }, new ProductGenre { GenreId = genres[2].Id } }, // Action, RPG
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 649, UnitsInStock = 30, Condition = "Ny", IsFeatured = true, SupplierId = supplier[2].Id } } 
        //          },
        //          new Product { 
        //              Title = "Super Mario Odyssey", CategoryId = categories[0].Id, 
        //              Description = "Följ med Mario på ett globalt äventyr för att rädda prinsessan Peach från Bowser.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[3].Id } }, // Platformer
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 499, UnitsInStock = 12, Condition = "Ny", IsFeatured = false, SupplierId = supplier[2].Id } } 
        //          },
        //          new Product { 
        //              Title = "Metroid Dread", CategoryId = categories[0].Id, 
        //              Description = "Samus Aran utforskar en mystisk planet och jagas av obevekliga E.M.M.I.-robotar.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id }, new ProductGenre { GenreId = genres[3].Id } }, // Action, Platformer
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 399, UnitsInStock = 0, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[2].Id } } 
        //          },
        //          new Product { 
        //              Title = "Resident Evil Revelations (Switch)", CategoryId = categories[0].Id, 
        //              Description = "Skräck ombord på ett övergivet kryssningsfartyg mitt ute på havet.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[1].Id }, new ProductGenre { GenreId = genres[4].Id } }, // FPS, Horror
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 320, UnitsInStock = 5, Condition = "Ny", IsFeatured = false, SupplierId = supplier[2].Id } } 
        //          },
        //          new Product { 
        //              Title = "Mario Kart 8 Deluxe", CategoryId = categories[0].Id, 
        //              Description = "Den definitiva versionen av Mario Kart med fler banor och karaktärer än någonsin.",
        //              ProductGenres = new List<ProductGenre> { new ProductGenre { GenreId = genres[0].Id } }, // Action
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 520, UnitsInStock = 18, Condition = "Ny", IsFeatured = true, SupplierId = supplier[2].Id } } 
        //          },
        //      
        //          // Konsoler
        //          new Product { 
        //              Title = "PlayStation 5 Digital", CategoryId = categories[1].Id, 
        //              Description = "Kraftfull konsol utan skivläsare, byggd för framtidens digitala spelande.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 5490, UnitsInStock = 3, Condition = "Ny", IsFeatured = true, SupplierId = supplier[0].Id } } 
        //          },
        //          new Product { 
        //              Title = "Xbox Series X", CategoryId = categories[1].Id, 
        //              Description = "Världens mest kraftfulla konsol med stöd för 4K-upplösning och snabba laddningstider.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 6199, UnitsInStock = 5, Condition = "Ny", IsFeatured = true, SupplierId = supplier[1].Id } } 
        //          },
        //          new Product { 
        //              Title = "Nintendo Switch OLED", CategoryId = categories[1].Id, 
        //              Description = "Spela var du vill med en fantastisk 7-tums OLED-skärm och förbättrad ljudkvalitet.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 4200, UnitsInStock = 10, Condition = "Ny", IsFeatured = true, SupplierId = supplier[2].Id } } 
        //          },
        //          new Product { 
        //              Title = "PlayStation 4 Pro", CategoryId = categories[1].Id, 
        //              Description = "Förbättrad prestanda för PS4-spel med stöd för 4K-upplösning.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 2200, UnitsInStock = 2, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[0].Id } } 
        //          },
        //          new Product { 
        //              Title = "Xbox Series S", CategoryId = categories[1].Id, 
        //              Description = "Kompakt men kraftfull, perfekt för nästa generations spelande i 1440p.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 2800, UnitsInStock = 0, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[1].Id } } 
        //          },
        //          //Tillbehör
        //          new Product { 
        //          Title = "DualSense Edge Controller", CategoryId = categories[2].Id, 
        //          Description = "En högpresterande handkontroll med utbytbara moduler och anpassningsbara knappar.",
        //          ProductItems = new List<ProductItem> { new ProductItem { Price = 2490, UnitsInStock = 4, Condition = "Ny", IsFeatured = true, SupplierId = supplier[0].Id } } 
        //           },
        //          new Product { 
        //              Title = "Xbox Elite Wireless Controller Series 2", CategoryId = categories[2].Id, 
        //              Description = "Byggd för prestation med justerbara styrspakar och paddlar på baksidan.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 1790, UnitsInStock = 0, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[1].Id } } 
        //          },
        //          new Product { 
        //              Title = "Nintendo Switch Pro Controller", CategoryId = categories[2].Id, 
        //              Description = "Klassisk handkontroll för Switch med rörelsestyrning och inbyggd amiibo-läsare.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 749, UnitsInStock = 15, Condition = "Ny", IsFeatured = true, SupplierId = supplier[2].Id } } 
        //          },
        //          new Product { 
        //              Title = "Pulse 3D Wireless Headset", CategoryId = categories[2].Id, 
        //              Description = "Finjusterat headset för 3D-ljud på PlayStation 5-konsoler.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 990, UnitsInStock = 8, Condition = "Ny", IsFeatured = false, SupplierId = supplier[0].Id } } 
        //          },
        //          new Product { 
        //              Title = "Xbox Wireless Headset", CategoryId = categories[2].Id, 
        //              Description = "Trådlöst headset med låg latens och exceptionell ljudkvalitet för Xbox och PC.",
        //              ProductItems = new List<ProductItem> { new ProductItem { Price = 850, UnitsInStock = 3, Condition = "Begagnad", IsFeatured = false, SupplierId = supplier[1].Id } } 
        //          }
        //              
        //              
        //          };
        //
        //         db.Products.AddRange(products);
        //         db.SaveChanges();
        //         
        //         Console.WriteLine($"Antal produkter: {db.Products.Count()}");
        //         Console.WriteLine($"Antal lagerartiklar: {db.ProductItems.Count()}");
        //         Console.WriteLine($"Antal genre-kopplingar: {db.ProductGenres.Count()}");
        //     }
        // }
        #endregion

        using (var db = new MyDbContext())
        {
            while (isRunning)
            { 
                Console.Clear();
                Lowest.LowestPosition = 0;
                
                Helpers.ShowDebugInfo(menuState);
                UIPage.GlobalLayout(menuState);
            
                switch (menuState)
                {
                    case MenuState.MainMenu:
                        UIPage.StartPage();
                        break;
                    case MenuState.CategoryMenu:
                        WindowExample.DrawCategoryMenu(db);
                        WindowExample.DrawProductMenu(db,selectedCategoryId);
                        break;
                    case MenuState.AdminMenu:
                        break;
                    case MenuState.CartMenu:
                        break;
                    case MenuState.Quit:
                        isRunning = false;
                        break;
                }
        
        
        
                //var windowStatus = new UX.Window("Systemstatus", 35, 20, new List<string> { lastAction });
                //windowStatus.Draw();

                //Console.SetCursorPosition(0, Lowest.LowestPosition);
                Console.WriteLine("Navigera genom att trycka på knapparna i fönstren [Tryck Q för att avsluta]");
        
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                char input = char.ToUpper(keyInfo.KeyChar);
                if (UIPage.KeyBindings.TryGetValue(input, out var binding))
                {
                    menuState = binding;
                    selectedCategoryId = 0;

                }
                else if (menuState == MenuState.CategoryMenu && char.IsDigit(input))
                {
                    int index = (int)char.GetNumericValue(input) - 1;
                    var categories = db.Categories.OrderBy(c => c.Id).ToList();

                    if (index >= 0 && index < categories.Count)
                    {
                        // Här mappar vi: Om användaren tryckte 1, hämtar vi ID:t för första kategorin (t.ex. 7)
                        selectedCategoryId = categories[index].Id;
                    }
                    
                }
                
                
                // switch (char.ToUpper(keyInfo.KeyChar))
                // {
                //
                //     case 'K':
                //         menuState = MenuState.CategoryMenu;
                //         lastAction = "Öppnar Kategorier";
                //         break;
                //     case 'A':
                //         lastAction = "Avslutar Shoppen";
                //         Console.Clear();
                //         var exitWindow = new UX.Window("Välkommen åter!", 45, 10, new List<string> { lastAction, "Tryck på valfri tangent..." });
                //         exitWindow.Draw();
                //         Console.ReadKey(true);
                //         isRunning = !isRunning;
                //         break;
                //     default:
                //         lastAction = $"Knapp '{keyInfo.KeyChar}' har ingen funktion än.";
                //         break;
                // }


            }
        
        }
    } 
}
        