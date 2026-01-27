using Spelshoppen.MenuPages;
using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class CategoryMenu : IMenuPage
{
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
        DrawWindows(db, session);
        UIRenderer.DrawNotifications(session);
        UIRenderer.DrawCategoryPrompts(session);
    }

    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        
        if (char.IsDigit(input))
        {
            ProcessIdInput(input, session, db);
        }

        //När användaren är på ProduktInformationen och Trycker på Enter så köper de varan.
        if (key.Key == ConsoleKey.Enter && session.SelectedProductId != 0)
        {
            var item = StoreServices.GetPurchasableItem(db,session.SelectedProductId);
            if (item != null) StoreServices.ExecutePurchase(db, session, item);
            session.SelectedProductId = 0;
        }
    }

    private static void ProcessIdInput(char input, UserSession session, MyDbContext db)
    {
        int id = InputHandler.PromptForId(input);
        
        if (session.SelectedCategoryId == 0)
        {
            if (!db.Categories.Any(c => c.Id == id))
            {
                session.NotificationMessage = $"Kategori {id} finns inte!";
                return;
            }

            session.SelectedCategoryId = id;
            return;
        }

        
        if (!db.Products.Any(p => p.Id == id && p.CategoryId == session.SelectedCategoryId))
        {
            session.NotificationMessage = $"Produkt {id} finns inte i denna kategori!";
            return;
        }

        session.SelectedProductId = id;
    }

    private static void DrawWindows(MyDbContext db, UserSession session)
    {
        DrawCategoryMenu(db);
        if (session.SelectedCategoryId != 0)
            DrawProductWindow(db, session.SelectedCategoryId);
        if (session.SelectedProductId != 0)
            DrawProductDetailsWindow(db, session.SelectedProductId);
    }

    private static void DrawCategoryMenu(MyDbContext db)
    {
        var categories = db.Categories.Select(c => $"[{c.Id}] {c.Title}").ToList();
        new UX.Window("KATEGORIER", 10, 8, categories).Draw();
    }

    private static void DrawProductWindow(MyDbContext db, int categoryId)
    {
        var products = db.Products.Where(p => p.CategoryId == categoryId)
            .Select(p => $"[{p.Id}] {p.Title}").ToList();
        new UX.Window("PRODUKTER", 30, 8, products).Draw();
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