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

        // Redigerar en specifik vara, ritar "EditingMenu"
        if (session.Status == CheckoutState.EditingItem)
        {
            var item = session.CartItem.Keys.FirstOrDefault(k => k.Id == session.SelectedProductId);
            if (item != null)
            {
                var editDetails = new List<string>
                {
                    $"PRODUKT: {item.Products?.Title}",
                    $"PRIS ST: {item.Price} kr",
                    $"ANTAL:   {session.CartItem[item]} st",
                    $"TOTALT:  {item.Price * session.CartItem[item]} kr",
                    "",
                    "[+] Öka antal",
                    "[-] Minska antal",
                    "[D] Ta bort från korg", //ÄNDRA I SAMBAND MED CRUD?
                    "[B] Gå tillbaka"
                };
                new UX.Window("ÄNDRA ANTAL", 15, 8, editDetails).Draw();
                return; 
            }
        }
        
        if (session.CartItem.Count == 0)
        {
            new UX.Window("VARUKORG", 15, 8,
                new List<string> { "Korgen är tom.", "Gå till [K]ategorier för att handla!" }).Draw();
            return;
        }
        
        //ID input för att ändra varukorgen
        var rows = session.CartItem.Select(kvp =>
            $"[{kvp.Key.Id}] {kvp.Key.Products?.Title,-15} {kvp.Value}st x {kvp.Key.Price,6} kr").ToList();
        
        rows.Add(Helpers.ShowXNumberOfLines(35));
        decimal total = session.CartItem.Sum(kvp => kvp.Key.Price * kvp.Value);
        rows.Add($"TOTALT: {total} kr");
        rows.Add("");
        rows.Add("Skriv [ID] för att ändra antal");
        rows.Add("[ENTER] BEKRÄFTA KÖP");

        new UX.Window("DIN VARUKORG", 15, 8, rows).Draw();
    }
    
    public void HandleInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        switch (session.Status)
        {
            case CheckoutState.ReviewingCart:
                HandleCartReview(key,input,session,db);
                break;

            case CheckoutState.EditingItem:
                HandleItemEdit(input, session);
                break;

            case CheckoutState.EnteringDetails:
                
                break;
        }
    }

    private void HandleCartReview(ConsoleKeyInfo key, char input, UserSession session,MyDbContext db)
    {
        if (char.IsDigit(input))
        {
            int id = InputHandler.PromptForId(input);
            if (session.CartItem.Keys.Any(k => k.Id == id))
            {
                session.SelectedProductId = id;
                session.Status = CheckoutState.EditingItem;
            }
            else
            {
                session.NotificationMessage = "ID hittades inte i varukorgen";
            }
        }
        else if (key.Key == ConsoleKey.Enter && session.CartItem.Any())
        {
            session.Status = CheckoutState.EnteringDetails;
            RunCheckout(session, db);
        }
    }

    private void HandleItemEdit(char input, UserSession session)
    {
        var item = session.CartItem.Keys.FirstOrDefault(k => k.Id == session.SelectedProductId);
        if (item == null)
        {
            session.Status = CheckoutState.ReviewingCart;
            return;
        }

        switch (input)
        {
            case '+':
                session.CartItem[item]++;
                item.UnitsInStock--;
                break;
            case '-':
                if (session.CartItem[item] > 1)
                {
                    session.CartItem[item]--;
                    item.UnitsInStock++;
                }

                break;
            case 'D': //Radera från varukorg
                item.UnitsInStock += session.CartItem[item];
                session.CartItem.Remove(item);
                session.Status = CheckoutState.ReviewingCart;
                break;
            case 'B': session.Status = CheckoutState.ReviewingCart; break;
        }
    }

    private void RunCheckout(UserSession session, MyDbContext db)
    {
        
        Helpers.UpdateAndSetCursorPosition();
        Console.WriteLine("KASSA: ANGE DINA UPPGIFTER:");

        try
        {
            session.FirstName = Prompt("Förnamn: ");
            session.LastName = Prompt("Efternamn: ");
            session.StreetName = Prompt("Gatuadress: ");
            session.CityName = Prompt("Stad: ");
            
            //Är Tvungen att inkludera Countries och Payment på grund av hur jag hade satt upp i min databas
            var countries = db.Countries.ToList();
            Console.WriteLine("\nVÄLJ LAND (ID):");
            foreach (var c in countries)
            {
                Console.WriteLine($"[{c.Id}] {c.Name}");
            }
            
            session.SelectedCountryId = InputHandler.PromptForId(Console.ReadKey(true).KeyChar);
            
            
            var payments = db.PaymentMethods.ToList();
            Console.WriteLine("\nVÄLJ BETALSÄTT (ID):");
            foreach (var p in payments) Console.WriteLine($"[{p.Id}] {p.Name}");
    
            session.SelectedPaymentMethodId = InputHandler.PromptForId(Console.ReadKey(true).KeyChar);

            // När alla uppgifter är insamlade, gå till nästa state
            session.Status = CheckoutState.ProcessOrder;

            // Anropa tjänsten som sköter databasjobbet
            OrderService.CreateOrder(db, session);
            session.CartItem.Clear();
            session.SelectedProductId = 0;
            session.Status = CheckoutState.ReviewingCart;
            session.NotificationMessage = "TACK FÖR DITT KÖP!";
        }
        catch (Exception)
        {
            session.NotificationMessage = "Inmatning avbröts.";
            session.Status = CheckoutState.ReviewingCart;
        }
    }

    
    private string Prompt(string message)
    {
        Console.Write(message);
        string input = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(input)) throw new Exception("Tom inmatning");
        return input;
    }
}