using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

public class SearchMenu : IMenuPage
{
    private string currentSearchTerm;
    private List<ProductSearchResult> searchResults = new List<ProductSearchResult>();
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);

        var searchRows = new List<string>();
        searchRows.Add($"{"ID:",-4} | {"TITEL:",-18} | {"PRIS",-10}");
        searchRows.Add(Helpers.PrintXNumberOfLines(40));
        if (searchResults.Any())
        {
            foreach (var item in searchResults)
            {
                searchRows.Add($"{item.ProductItemId,-4} | {item.Title,-18} | {item.Price,-10}");
            }
        }
        else
        {
            searchRows.Add("");
            searchRows.Add(string.IsNullOrEmpty(currentSearchTerm) ? "Tryck [S] för att starta en sökning" 
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
                ExecuteSearch(session);
                break;
        }
    }

    private void ExecuteSearch(UserSession session)
    {
        try
        {
            Helpers.UpdateAndSetCursorPosition();
            string searchTerm = Helpers.Prompt("SÖK PRODUKT: ");
            currentSearchTerm = searchTerm;
            searchResults = SearchProduct.SearchProducts(searchTerm);
            if (searchResults.Count > 0)
            {
                session.NotificationMessage = $"Hittade {searchResults.Count} matchningar: ";
            }
            
            
        }
        catch(OperationCanceledException)
        {
            session.NotificationMessage = "SÖKNING AVBRUTEN";
        }
        UIRenderer.DrawBaseLayout(session);

    }
}