using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

public class AdminMenu : IMenuPage
{
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
        DrawWindows(db, session); 
        var rows = new List<string>() { "[L] LÄGG TILL", "[P] PROGNOS" };
        new UX.Window("ADMIN", 20, 1, rows).Draw(); 
        UIRenderer.DrawNotifications(session);
        UIRenderer.DrawCategoryPrompts(session);
    }

    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        
        if(char.IsDigit(input)) InputHandler.HandleNavigation(db,session,input);

        
            var command = char.ToUpper(input);
            switch (command)
            {
                case 'L': AddProduct(db, session); break;
                case 'P': GetStatistics(db); break;
            }

            if (session.SelectedProductId != 0)
            {
                switch (command)
                {
                    case 'E': 
                        ManageFeaturedProducts(db,session,session.SelectedProductId); 
                        
                        break;
                    case 'U':
                        UpdateProduct(db, session, session.SelectedProductId);
                        break;
                    case 'R':
                        DeleteProduct(db, session, session.SelectedProductId);
                        session.SelectedProductId = 0;
                        break;
                }
            }
    }

    private static void ManageFeaturedProducts(MyDbContext db, UserSession session,int productId)
    {
        bool success = StoreServices.ToggleFeaturedStatus(db, productId);
        var product = db.Products.Find(productId);
    
        session.NotificationMessage = success 
            ? $"{product?.Title} uppdaterad!" 
            : "Max 3 erbjudanden tillåtna!";
        
    }

    private static void DrawFeaturedProductList(MyDbContext db, int categoryId)
    {
        var products = db.ProductItems
            .Include(pi => pi.Products)
            .Where(pi => pi.Products.CategoryId == categoryId)
            .ToList();

        var rows = new List<string>();
        rows.Add($"{"ID",-4} | {"TITEL",-25} | {"STATUS"}");
        

        foreach (var item in products)
        {
            string status = item.IsFeatured ? "[X]" : "[ ]";
            string title = item.Products.Title.Length > 25 
                ? item.Products.Title.Substring(0, 22) + "..." 
                : item.Products.Title;

            rows.Add($"{item.Id,-4} | {title,-25} | {status}");
        }

        new UX.Window("PRODUKTER (NAVIGERA MED ID)", 35, 8, rows).Draw();
    }

    //TODO: REFACTOR
    private static void GetStatistics(MyDbContext db)
    {
        var outOfStockCount = db.ProductItems.Count(pi => pi.UnitsInStock == 0);
        var mostStockCount = db.ProductItems.OrderByDescending(pi =>  pi.UnitsInStock).
            Select(pi => $"{pi.Products.Title} [{pi.UnitsInStock} st]").FirstOrDefault() ?? "Okänt";
        var mostGenresInGames = db.Products
            .OrderByDescending(p => p.ProductGenres.Count())
            .Select(p => $"{p.Title} [{p.ProductGenres.Count()} st]")
            .FirstOrDefault() ?? "N/A";
        
        var reportRows = new List<string>()
        {
            $"Antalet slut i lagret: {outOfStockCount}",
            $"Störst lager: {mostStockCount}",
            $"Flest genres: {mostGenresInGames}",
            "",
            "Tryck på valfri tangent för att gå tillbaks."
        };
        new UX.Window("PROGNOS",20,10,reportRows).Draw();
        Console.ReadKey(true);
    }
    
    
    private static void AddProduct(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
        //Helpers.UpdateAndSetCursorPosition();

        //Väljer kategori ID
        var categoryList = db.Categories.Select(c => $"[{c.Id}] {c.Title}").ToList();
        new UX.Window("LÄGG TILL: VÄLJ KATEGORI-ID", 20, 10, categoryList).Draw();
        
        int.TryParse(Helpers.Prompt("Kategori-ID"), out var categoryId);

        List<int> selectedGenres = new List<int>();
        UIRenderer.DrawBaseLayout(session);
        //Kollar om Admin väljer spel
        if (categoryId == 1)
        {
            //Listar upp alla genres i databasen
            var genreList = db.Genres.Select(g => $"[{g.Id}] {g.Name}").ToList();
            new UX.Window("VÄLJ GENRE-ID", 55, 10, genreList).Draw();

            //Kollar om admin skriver en eller flera genres
            string genreIdInput = Helpers.Prompt("Välj Genre IDs [SEPARERA MED ',' VID FLER VAL]");
            selectedGenres = genreIdInput.Split(',')
                .Select(s => int.TryParse(s.Trim(), out int id) ? id : 0)
                .Where(id => id > 0).ToList();
        }
        UIRenderer.DrawBaseLayout(session);
        string title = Helpers.Prompt("Namn på Objektet");
        string description = Helpers.Prompt("Beskrivning");
        decimal price = decimal.Parse(Helpers.Prompt("Pris"));
        int stock = int.Parse(Helpers.Prompt("Antal i lager"));
        string condition = Helpers.Prompt("Skick?");
        var firstCategory = db.Categories.Select(c => c.Id).FirstOrDefault();
        var firstSupplier = db.Suppliers.Select(s => s.Id).FirstOrDefault();

        if (firstCategory == 0 || firstSupplier == 0)
        {
            session.NotificationMessage = "FELMEDDELANDE: du måste ha minst en kategori och en leverantör i databasen!";
            return;
        }

        AdminService.AddProduct(db, title, description, price, 
            stock, categoryId, selectedGenres, firstSupplier, condition);
        session.NotificationMessage = $"La in titeln: {title}";
    }

    private void UpdateProduct(MyDbContext db, UserSession session, int id)
    {
        var item = db.ProductItems.Include(pi => pi.Products)
            .FirstOrDefault(pi => pi.Id == id);
        if (item == null) return;
        
        UIRenderer.DrawBaseLayout(session);
        
        var options = new List<string>
        {
            $"REDIGERAR: {item.Products?.Title}",
            "[1] Ändra Titel",
            "[2] Ändra Pris",
            "[3] Ändra Lager",
            "[4] Ändra Skick",
        };
        new UX.Window("UPPDATERA PRODUKT", 40, 10, options).Draw();
        
        //Val av ändra produkt
        var choice = Console.ReadKey(true).KeyChar;
        switch (choice)
        {
            case '1':
                item.Products.Title = Helpers.Prompt("Ny titel"); break;
            case '2':
                if (decimal.TryParse(Helpers.Prompt("Nytt pris"), out var price))
                    item.Price = price;
                break;
            case '3':
                if (int.TryParse(Helpers.Prompt("Ändra lagersaldo"), out var stock))
                    item.UnitsInStock = stock;
                break;
            case '4':
                item.Condition = Helpers.Prompt($"Ändra skick [Nuvarande Skick: {item.Condition}]");
                break;
            default:
                return;
        }
        db.SaveChanges();
        session.NotificationMessage = "Ändringen har sparats!";
    }

    private static void DeleteProduct(MyDbContext db, UserSession session, int id)
    {
        UIRenderer.DrawBaseLayout(session);
        
        var product = db.ProductItems
            .Include(pi => pi.Products)
            .FirstOrDefault(pi => pi.Id == id);

        if (product == null) return;

        //Skriver ut titeln beroende på vilket ID jag har valt
        string title = product.Products?.Title ?? "Okänd titel";
        string confirmation = Helpers.Prompt($"Är du säker på att du vill ta bort {title}? [Skriv 'J' för JA]");
        //Console.Write($"Är du säker på att du vill ta bort {title}?");
        if (confirmation.ToUpper() == "J")
        {
            bool success = AdminService.DeleteProduct(db, id);
            session.NotificationMessage = success ? $"ID {title} har raderats" : $"Hittade inte ID";
        }
        else
        {
            session.NotificationMessage = $"Avbryter radering";
        }
    }
    private static void DrawWindows(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawCategoryMenu(db,"KATEGORIER");
        
        if (session.SelectedCategoryId != 0)
        {
            DrawFeaturedProductList(db,session.SelectedCategoryId);
        }

        if (session.SelectedProductId != 0)
        {
            DrawAdminWindow(db, session.SelectedProductId);
        }
    }
    private static void DrawAdminWindow(MyDbContext db, int productId)
    {
        var product = StoreServices.GetFullProduct(db,productId);
        var item = product?.ProductItems.FirstOrDefault();
        
        var rows = new List<string>
        {
            $"VALD PRODUKT: {product?.Title}",
            $"LAGER:        {item?.UnitsInStock} st",
            $"PRIS:         {item?.Price} kr",
            $"{Helpers.PrintXNumberOfLines(25)}",
            "HANTERA:",
            "[U] UPPDATERA DATA",
            "[R] RADERA PRODUKT",
            "[E] ÄNDRA ERBJUDANDE STATUS",
            "",
            "[0] TILLBAKA"
        };
        new UX.Window("ADMIN: PRODUKTINFORMATION", 80, 8, rows).Draw();
        
    }
}