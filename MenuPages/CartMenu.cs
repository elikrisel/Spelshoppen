using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;
using Spelshoppen.Transactions;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

public class CartMenu : IMenuPage
{
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
        UIRenderer.DrawNotifications(session);
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
                    $"{Helpers.PrintXNumberOfLines(25)}",
                    "[+] Öka antal",
                    "[-] Minska antal",
                    "[D] Ta bort från korg", 
                    "[B] Gå tillbaka"
                };
                new UX.Window("ÄNDRA ANTAL", 15, 8, editDetails).Draw();
                return; 
            }
        }
        //Om kunden inte har lagt in något i varukorgen
        if (session.CartItem.Count == 0)
        {
            new UX.Window("VARUKORG", 15, 8,
                new List<string> { "Korgen är tom.", "Gå till [K]ategorier för att handla!" }).Draw();
            return;
        }
        
        //ID input för att ändra varukorgen
        var rows = session.CartItem.Select(kvp =>
            $"[{kvp.Key.Id}] {kvp.Key.Products?.Title,-15} {kvp.Value}st x {kvp.Key.Price,6} kr").ToList();
        
        rows.Add(Helpers.PrintXNumberOfLines(35));
        decimal total = session.CartItem.Sum(kvp => kvp.Key.Price * kvp.Value);
        rows.Add($"TOTALT: {total} kr");
        rows.Add("");
        rows.Add("Skriv [ID] för att ändra antal");
        rows.Add("[ENTER] BEKRÄFTA KÖP");

        new UX.Window("DIN VARUKORG", 15, 8, rows).Draw();
    }
    
    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
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
        {   //Letar input beroende på vad för produkt som finns i varukorgen
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
    {   //Väljer en produkt att redigera
        var item = session.CartItem.Keys.FirstOrDefault(k => k.Id == session.SelectedProductId);
        if (item == null)
        {
            session.Status = CheckoutState.ReviewingCart;
            return;
        }

        switch (input)
        {
            case '+':
                if (item.UnitsInStock > 0) 
                {
                    session.CartItem[item]++;
                    item.UnitsInStock--;
                }
                else 
                {
                    session.NotificationMessage = "Lagret är tyvärr slut!";
                }
                break;
            case '-':
                if (session.CartItem[item] > 1)
                {
                    session.CartItem[item]--;
                    item.UnitsInStock++;
                }
                else if (session.CartItem[item] == 1)
                {
                    session.CartItem.Remove(item); 
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
        
        
        Console.WriteLine("KASSA: ANGE DINA UPPGIFTER:");

        try
        {   //Skriver in information
            session.FirstName = Helpers.Prompt("Förnamn");
            session.LastName = Helpers.Prompt("Efternamn");
            session.StreetName = Helpers.Prompt("Gatuadress");
            session.CityName = Helpers.Prompt("Stad");
            
            //Är Tvungen att inkludera Countries och Payment på grund av hur jag hade satt upp i min databas
            var countries = db.Countries.ToList();
            //Kollar om det finns ett land
            if (countries.Any())
            {
                Console.WriteLine("Välj Land-ID:\n");
                foreach (var c in countries)
                {
                    Console.WriteLine($"[{c.Id}][{c.Name}]");
                }
                session.SelectedCountryId = InputHandler.PromptForId(Console.ReadKey(true).KeyChar);
                session.CountryName = countries.FirstOrDefault(c => c.Id == session.SelectedCountryId)?.Name ?? "";           
            }
            else
            {
                //Om inte så får användaren mata in
                session.CountryName = Helpers.Prompt("Ange land");
                session.SelectedCountryId = 0;
            }
            
            //Samma funktionalitet som Countries
            var payments = db.PaymentMethods.ToList();
            if (payments.Any())
            {
                Console.WriteLine("\nVÄLJ BETALSÄTT (ID):");
                foreach (var p in payments)
                {
                    Console.WriteLine($"[{p.Id}] {p.Name}");
                } 
                session.SelectedPaymentMethodId = InputHandler.PromptForId(Console.ReadKey(true).KeyChar);
                session.PaymentMethodName = payments.FirstOrDefault(p => p.Id == session.SelectedPaymentMethodId)
                    ?.Name ?? "";
            }
            else
            {
                session.PaymentMethodName = Helpers.Prompt("Ange betalning");
                session.SelectedPaymentMethodId = 0;
            }
    
            // När alla uppgifter är insamlade, gå till nästa state
            session.Status = CheckoutState.ProcessOrder;
            
            OrderService.CreateOrder(db, session);
            session.NotificationMessage = "TACK FÖR DITT KÖP!";
            session.CartItem.Clear();
            session.SelectedProductId = 0;
            session.Status = CheckoutState.ReviewingCart;
        }
        catch (Exception)
        {
            session.NotificationMessage = "Kunde inte spara ordern. Försök igen senare";
            session.Status = CheckoutState.ReviewingCart;
            Console.WriteLine("Kunde inte spara ordern!");
        }
    }
    
}