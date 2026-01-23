using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;
using Spelshoppen.Transactions;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;


public class CartMenu : IMenuPage
{
    public void Draw(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);

        if (session.Cart.Count == 0)
        {
            new UX.Window("VARUKORG", 15, 8, new List<string> { "Korgen är tom.", "Gå till [K]ategorier för att handla!" }).Draw();
            return;
        }

        var rows = session.Cart.Select(i => $"{i.Products?.Title,-20} {i.Price,8} kr").ToList();
        rows.Add(new string('-', 30));
        rows.Add($"TOTALT: {session.Cart.Sum(i => i.Price)} kr");
        rows.Add("");
        rows.Add("[ENTER] BEKRÄFTA KÖP");
        rows.Add("[R] AVBRYT KÖP");

        new UX.Window("DIN VARUKORG", 15, 8, rows).Draw();
    }
    
    
    //TODO: REFAKTURERA KODEN
    public void HandleInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        // 1. Grundläge: Vänta på Enter för att starta kassan
        if (session.CheckoutStep == 0)
        {
            if (key.Key == ConsoleKey.Enter && session.Cart.Any())
            {
                session.CheckoutStep = 1;
            }
            else if (input == 'R') 
            {
                session.Cart.Clear();
                session.NotificationMessage = "Korgen tömd.";
                return;
            }
            else return;
        }
        
        Console.SetCursorPosition(0, UX.Lowest.LowestPosition + 2);

        if (session.CheckoutStep == 1)
        {
            Console.Write("Ange förnamn: ");
            session.FirstName = Console.ReadLine();
            session.CheckoutStep = 2;
        }

        if (session.CheckoutStep == 2)
        {
            Console.Write("Ange efternamn: ");
            session.LastName = Console.ReadLine();
            session.CheckoutStep = 3;
        }

        if (session.CheckoutStep == 3)
        {
            Console.Write("Ange gatuaddress: ");
            session.StreetName = Console.ReadLine();
            session.CheckoutStep = 4;
        }

        if (session.CheckoutStep == 4)
        {
            Console.Write("Ange Stad: ");
            session.CityName = Console.ReadLine();
            
            FinalizeOrder(db, session);
            
            while (Console.KeyAvailable) Console.ReadKey(true);
        }
    }

 private void FinalizeOrder(MyDbContext db, UserSession session)
{
    try 
    {
        // Hämtar landet som ligger sparat i OnModelCreating
        var country = db.Countries.FirstOrDefault(c => c.Id == 1) 
                      ?? db.Countries.FirstOrDefault(c => c.Name == "Sverige");

        //Hanterar Stad enligt input från användaren
        var city = db.Cities.FirstOrDefault(c => c.Name == session.CityName)
                   ?? new City { Name = session.CityName, Country = country };

        //Hanterar kunden
        var customer = db.Customers.FirstOrDefault(c => 
                           c.FirstName == session.FirstName && c.LastName == session.LastName) 
                       ?? new Customer { 
                           FirstName = session.FirstName, 
                           LastName = session.LastName, 
                           Street = session.StreetName 
                       };

        //Hämtar betalningsmetod, vilket är faktura.
        var payment = db.PaymentMethods.FirstOrDefault(p => p.Id == 1) 
                      ?? db.PaymentMethods.FirstOrDefault(p => p.Name == "Faktura");

        //Skapar ordern
        var order = new Order
        {
            Customers = customer,
            Cities = city,
            PaymentMethods = payment, 
            TotalAmount = session.Cart.Sum(i => i.Price),
            OrderDate = DateTime.Now,
            Street = session.StreetName
        };

        //Skapar order raden
        foreach (var item in session.Cart)
        {
            //Kopplar order med orderradern
            var line = new OrderLine
            {
                Orders = order, 
                ProductItemId = item.Id,
                Quantity = 1,
                TotalPrice = item.Price,
                VatRate = 0.25m
            };
            db.OrderLines.Add(line);
        }
        
        //Lägger till och sparar ordern
        db.Orders.Add(order);
        db.SaveChanges(); 

        //Rensar shoppen och går tillbaks till menyn
        session.Cart.Clear();
        session.CheckoutStep = 0;
        session.State = MenuState.MainMenu;
        session.NotificationMessage = "Ordern har sparats i databasen!";
    }
    catch (Exception ex)
    {
        
        string error = ex.InnerException?.Message ?? ex.Message;
        session.NotificationMessage = "Kunde inte spara: " + error;
        
        session.CheckoutStep = 0;
        session.State = MenuState.MainMenu;
        
        Console.WriteLine("\nVARNING, FEL MED SPARA I DATABASEN: " + error);
        Console.ReadKey();
    }
}
 
}