using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

public class CartMenu : IMenuPage
{
    public void Draw(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);

        if (session.Cart.Count == 0)
        {
            new UX.Window("VARUKORG", 15, 8, new List<string> { 
                "Korgen är tom.", 
                "Gå till [K]ategorier för att handla!" 
            }).Draw();
            return;
        }

        var rows = session.Cart.Select(i => $"{i.Products?.Title,-20} {i.Price,8} kr").ToList();
        rows.Add(Helpers.ShowXNumberOfLines(30));
        rows.Add($"TOTALT: {session.Cart.Sum(i => i.Price)} kr");
        rows.Add("");
        rows.Add("[ENTER] BEKRÄFTA KÖP OCH SKAPA ORDER");
        rows.Add("[R] AVBRYT KÖP");

        new UX.Window("DIN VARUKORG", 15, 8, rows).Draw();
        
    }

    public void HandleInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        if (key.Key == ConsoleKey.Enter && session.Cart.Any())
        {
            ProcessOrder(db, session);
        }

        // 2. Avbryt köp (Lägg tillbaka ALLA produkter i lagret)
        if (input == 'R')
        {
            CancelOrderAndRestore(db, session);
        }
    }

    private void ProcessOrder(MyDbContext db, UserSession session)
    {
        session.NotificationMessage = $"Order lagd!";
        session.Cart.Clear();
        session.State = MenuState.MainMenu;
    }

    private void CancelOrderAndRestore(MyDbContext db, UserSession session)
    {
        foreach (var item in session.Cart)
        {
            item.UnitsInStock++;
        }

        db.SaveChanges();
        session.Cart.Clear();
        session.NotificationMessage = $"Köp avbrutet och lagret har återställts!";

    }
    
    
    
}