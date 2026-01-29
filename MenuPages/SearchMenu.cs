using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

public class SearchMenu : IMenuPage
{
    
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);

        var searchRows = new List<string>();
        searchRows.Add($"{"ID:",-4} | {"TITEL:",-18} | {"PRIS",-10}");
        searchRows.Add(Helpers.PrintXNumberOfLines(40));
        
        if (session.SearchResults.Any())
        {
            foreach (var item in session.SearchResults)
            {
                searchRows.Add($"{item.ProductItemId,-4} | {item.Title,-18} | {item.Price,-10}");
            }
        }
        else
        {
            searchRows.Add("");
            searchRows.Add(string.IsNullOrEmpty(session.CurrentSearchterm) ? "Tryck [F] för att starta en sökning" 
                : "Inga träffar, försök igen!");
        }
        searchRows.Add("");
        searchRows.Add("[F] SÖK PÅ NYTT   [M] MENY");
        new UX.Window("SÖK PRODUKTER", 15, 8, searchRows).Draw();
        
    }

    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        switch (char.ToUpper(key.KeyChar))
        {
            case 'F':
                ExecuteSearch(session,db);
                break;
        }
    }

    private void ExecuteSearch(UserSession session,MyDbContext db)
    {
        session.ClearSearch();
        try
        {
           
            string searchTerm = Helpers.Prompt("SÖK PRODUKT");
            session.CurrentSearchterm = searchTerm;
            session.SearchResults = SearchProduct.SearchProducts(searchTerm,db);
            if (session.SearchResults.Count > 0)
            {
                session.NotificationMessage = $"Hittade {session.SearchResults.Count} matchningar: ";
            }
        }
        catch(OperationCanceledException)
        {
            session.NotificationMessage = "SÖKNING AVBRUTEN";
        }
        UIRenderer.DrawBaseLayout(session);
    }
}