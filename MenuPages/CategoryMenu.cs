using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class CategoryMenu
{

    public static void Draw(MyDbContext db, UserSession session)
    {
        DrawWindows(db,session);
        UIRenderer.DrawNotifications(session);
        UIRenderer.DrawPrompts(session);
    }

    private static void DrawWindows(MyDbContext db, UserSession session)
    {
        DrawCategoryMenu(db);
        if(session.SelectedCategoryId != 0)
            DrawProductWindow(db,session.SelectedCategoryId);
        if(session.SelectedProductId != 0)
            DrawProductDetailsWindow(db, session.SelectedProductId);
    }

    private static void DrawCategoryMenu(MyDbContext db)
    {
        var categories = db.Categories.Select(c => $"[{c.Id}] {c.Title}").ToList();
        new UX.Window("Kategorier", 10, 8, categories).Draw();    
    }
    
    private static void DrawProductWindow(MyDbContext db, int categoryId)
    {
        var products = db.Products.Where(p => p.CategoryId == categoryId)
            .Select(p => $"[{p.Id}] {p.Title}").ToList();
        new UX.Window("Produkter", 30, 8, products).Draw();
    }

    private static void DrawProductDetailsWindow(MyDbContext db, int productId)
    {
        var product = StoreServices.GetFullProduct(db,productId);
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