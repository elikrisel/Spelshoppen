using Spelshoppen.MenuPages;
using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class CategoryMenu : IMenuPage
{
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
        DrawWindows(db, session);
        UIRenderer.DrawCategoryPrompts(session);
    }

    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        
        if (char.IsDigit(input)) InputHandler.HandleNavigation(db, session, input);
        

        //När användaren är på ProduktInformationen och Trycker på Enter så köper de varan.
        if (key.Key == ConsoleKey.Enter && session.SelectedProductId != 0)
        {
            var item = StoreServices.GetPurchasableItem(db,session.SelectedProductId);
            if (item != null) StoreServices.ExecutePurchase(db, session, item);
            session.SelectedProductId = 0;
        }
    }

    private static void DrawWindows(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawCategoryMenu(db,"KATEGORIER");
        if (session.SelectedCategoryId != 0)
            UIRenderer.DrawProductWindow(db, session.SelectedCategoryId);
        if (session.SelectedProductId != 0)
            DrawProductDetailsWindow(db, session.SelectedProductId);
    }
    
    private static void DrawProductDetailsWindow(MyDbContext db, int productId)
    {
        var product = StoreServices.GetFullProduct(db, productId);
        if (product == null) return;

        var item = product.ProductItems.FirstOrDefault();
        var genres = string.Join(", ", product.ProductGenres.Select(pg => pg.Genres.Name));

        var productDetails = new List<string>
        {
            $"Titel:  {product.Title}",
            $"Genre:  {genres}",
            $"Pris:   {item?.Price} kr",
            $"Skick:  {item?.Condition}",
            $"Lager:  {item?.UnitsInStock} st",
            "",
            "BESKRIVNING:",
            product.Description ?? "INGEN BESKRIVNING TILLGÄNGLIG",
            "",
            item?.UnitsInStock > 0 ? "[ENTER] LÄGG I VARUKORG" : "SLUT I LAGER"
        };
        new UX.Window("PRODUKTINFORMATION", 75, 8, productDetails).Draw();
    }
}