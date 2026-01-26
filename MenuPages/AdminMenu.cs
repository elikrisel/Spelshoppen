using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

//TODO: FIX ADMIN MENU
public class AdminMenu : IMenuPage
{
    public void Draw(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);

        var inventory = db.ProductItems.Select(pi => new
            {
                pi.Id,
                Title = pi.Products!.Title ?? "Okänd",
                Description = pi.Products.Description ?? "Ingen beskrivning",
                Publisher = pi.Suppliers.PublisherName ?? "Okänd",
                Condition = pi.Condition ?? "Ny",
                pi.Price,
                pi.UnitsInStock
            }).OrderByDescending(pi => pi.Id)
            .Take(5) //Visar fem senaste med anledning av att UI Window bråkar när man läser upp allting
            .ToList();

        var rows = new List<string>();
        rows.Add($"{"ID",-4} | {"TITEL",-18} | {"UTGIVARE",-15} |  {"PRIS",-8} | {"LAGER"} ");
        rows.Add(Helpers.ShowXNumberOfLines(rows.Count));
        foreach (var i in inventory)
        {
            rows.Add($"{i.Id,-3} | {i.Title,-18} | {i.Publisher,-12} | {i.Price,6}kr | {i.UnitsInStock,2}st");
        }

        rows.Add("");
        rows.Add("[L] LÄGG TILL   [U] 'UPPDATERA' PRODUKT   [R] RADERA");

        new UX.Window("ADMIN: LAGERHANTERING", 15, 8, rows).Draw();
        UIRenderer.DrawNotifications(session);
    }

    public void HandleInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        int targetId;
        switch (char.ToUpper(input))
        {
            case 'L':
                AddProduct(db, session);
                break;
            case 'U':
                
                Console.Write("SKRIV ID SOM DU VILL UPPDATERA: ");
                targetId = InputHandler.PromptForId(Console.ReadKey(true).KeyChar);
                UpdateProduct(db,session,targetId);
                break;
            case 'R':
                targetId = InputHandler.GetAdminIdInput("SKRIV ID SOM DU VILL TA BORT: ");
                DeleteProduct(db, session,targetId);
                break;
        }
    }

    private static void AddProduct(MyDbContext db, UserSession session)
    {
        Helpers.UpdateAndSetCursorPosition();

        //Väljer kategori ID
        var categorySelect = db.Categories.ToList();
        Console.WriteLine("Kategorier: " + string.Join(',', categorySelect.Select(c => $"[{c.Id}] {c.Title}")));
        Console.Write("Välj Kategori ID: ");
        int.TryParse(Console.ReadLine(), out var categoryId);


        List<int> selectedGenres = new List<int>();

        //Kollar om Admin väljer spel
        if (categoryId == 1)
        {
            //Listar upp genres
            var genres = db.Genres.ToList();
            Console.WriteLine("Genres: " + string.Join(',', genres.Select(g => $"[{g.Id}] {g.Name}")));
            Console.Write("Välj Genre IDs: ");
            string genreInput = Console.ReadLine() ?? "";

            //Kollar om man skriver mer än en genre ID
            selectedGenres = genreInput.Split(',')
                .Select(s => int.TryParse(s.Trim(), out int id) ? id : 0)
                .Where(id => id > 0).ToList();
        }


        Console.Write("Namn på Objektet:");
        string title = Console.ReadLine() ?? "Okänd titel";

        Console.Write("Beskrivning: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("Pris: ");
        decimal.TryParse(Console.ReadLine(), out var price);

        Console.Write("Antal i lager: ");
        int.TryParse(Console.ReadLine(), out var stock);

        Console.Write("Skick?: ");
        string condition = Console.ReadLine() ?? "Ny";

        //Hämtar första bästa Kategori och Supplier från databasen
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

    private void UpdateProduct(MyDbContext db, UserSession session,int id)
    {
        
            var item = db.ProductItems.Include(pi => pi.Products)
                .FirstOrDefault(pi => pi.Id == id);

            if (item == null)
            {
                session.NotificationMessage = $"ID {id} HITTADES INTE!";
                return;
            }

            Console.WriteLine($"\nREDIGERAR [{id}] {item.Products?.Title}");
            Console.WriteLine("[1] Ändra Titel [2] Ändra Pris [3] Ändra Lager [4] Ändra Skick [5] Avbryt");

            var choice = Console.ReadKey(true).KeyChar;

            switch (choice)
            {
                case '1':
                    Console.Write("Ny titel: ");
                    item.Products.Title = Console.ReadLine() ?? item.Products.Title;
                    break;
                case '2':
                    Console.Write("Nytt pris: ");
                    if(decimal.TryParse(Console.ReadLine(), out var price)) item.Price = price;
                    break;
                case '3':
                    Console.Write("Ändra lagersaldo: ");
                    if(int.TryParse(Console.ReadLine(), out var stock)) item.UnitsInStock = stock;
                    break;
                case '4':
                    Console.Write($"Ändra skick (Nuvarande: {item.Condition}): ");
                    string newCondition = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newCondition)) 
                    {
                        item.Condition = newCondition;
                    }
                    break;
                case '5':
                    session.NotificationMessage = "Ändring avbruten.";
                    break;
                default:
                    return;
            }

            db.SaveChanges();
            session.NotificationMessage = "Ändringen har sparats!";


    }

    private static void DeleteProduct(MyDbContext db, UserSession session, int id)
    {
        
            var product = db.ProductItems
                .Include(pi => pi.Products)
                .FirstOrDefault(pi => pi.Id == id);

            if (product == null) return;
            
            //Skriver ut titeln beroende på vilket ID jag har valt
            string title = product.Products?.Title ?? "Okänd titel";
            Console.Write($"Är du säker på att du vill ta bort {title}?");
            
            if (char.ToUpper(Console.ReadKey().KeyChar) == 'J')
            {
                bool success = AdminService.DeleteProduct(db, id);
                session.NotificationMessage = success ? $"ID {title} har raderats" : $"Hittade inte ID";
            }
            else
            {
                session.NotificationMessage = $"Avbryter radering";
            }
        
    }
}