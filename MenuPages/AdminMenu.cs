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
        rows.Add("[L] LÄGG TILL   [P] 'ÄNDRA' PRIS   [R] RADERA");

        new UX.Window("ADMIN: LAGERHANTERING", 15, 8, rows).Draw();
        UIRenderer.DrawNotifications(session);
    }

    public void HandleInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        switch (char.ToUpper(input))
        {
            case 'L':
                AddProduct(db, session);
                break;
            case 'P':
                break;
            case 'R':
                DeleteProduct(db, session);
                break;
        }
    }

    private static void AddProduct(MyDbContext db, UserSession session)
    {
        Console.SetCursorPosition(0, Lowest.LowestPosition + 2);

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

    private static void DeleteProduct(MyDbContext db, UserSession session)
    {
        Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("ANGE ID PÅ PRODUKTEN SOM DU VILL RADERA: ");
        Console.ResetColor();

        
        if (int.TryParse(Console.ReadLine(), out var id))
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
}