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

        }).OrderByDescending(pi => pi.Id) // Visa de nyaste överst
        .Take(5) // Hämta bara de 10 första för att rymmas på skärmen
        .ToList();

        var rows = new List<string>();
        rows.Add($"{"ID",-4} | {"TITEL",-18} | {"UTGIVARE",-15} |  {"PRIS",-8} | {"LAGER"} ");
        rows.Add(Helpers.ShowXNumberOfLines(40));
        foreach (var i in inventory)
        {
            
            rows.Add($"{i.Id,-3} | {i.Title,-18} | {i.Publisher,-12} | {i.Price,6}kr | {i.UnitsInStock,2}st");
            
        }
        rows.Add("");
        rows.Add("[L] LÄGG TILL   [P] 'ÄNDRA' PRIS   [R] RADERA");
        
        new UX.Window("ADMIN: LAGERHANTERING",15,8,rows).Draw();
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
                break;
        }
    }

    private static void AddProduct(MyDbContext db, UserSession session)
    {
        Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
        Console.WriteLine("LÄGG TILL PRODUKT:");
        
        
        Console.Write("Namn på Spelet:");
        string title = Console.ReadLine() ?? "Okänd titel";
        
        Console.Write("Beskrivning: ");
        string description = Console.ReadLine() ?? "";
        
        Console.Write("Pris: ");
        decimal.TryParse(Console.ReadLine(), out var price);
        
        Console.Write("Antal i lager: ");
        int.TryParse(Console.ReadLine(), out var stock);
        
        Console.Write("Skick?");
        string condition = Console.ReadLine() ?? "Ny";
        
        //Hämtar första bästa Kategori och Supplier från databasen
        var firstCategory = db.Categories.Select(c => c.Id).FirstOrDefault();
        var firstSupplier = db.Suppliers.Select(s => s.Id).FirstOrDefault();

        if (firstCategory == 0 || firstSupplier == 0)
        {
            session.NotificationMessage = "FELMEDDELANDE: du måste ha minst en kategori och en leverantör i databasen!";
            return;
        }
        AdminService.AddProduct(db,title,description,price,stock,firstCategory,firstSupplier,condition);
        session.NotificationMessage = $"La in titeln: {title}";


    }
    
    
}