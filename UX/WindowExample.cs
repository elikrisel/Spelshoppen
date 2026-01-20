using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class WindowExample
{
    public static void DrawCategoryMenu(MyDbContext db)
    {
        var categories = db.Categories.OrderBy(c => c.Id).ToList();
        List<string> categoryRows = new List<string>();

        foreach (var cat in categories)
        {
            categoryRows.Add($"[{cat.Id}] {cat.Title}");
        }

        var categoryWindow = new UX.Window("Kategorier:", 20, 8, categoryRows);
        categoryWindow.Draw();
        
    }

    public static void DrawProductMenu(MyDbContext db, int selectedId)
    {
        if (selectedId <= 0) return;
        var categoryName = db.Categories.FirstOrDefault(c => c.Id == selectedId)?.Title;

        var products = db.Products.Where(p => p.CategoryId == selectedId).ToList();
        List<string> productRows = products.Select(p => $"{p.Id}: {p.Title}").ToList();

        var productWindow = new UX.Window($"{categoryName}", 40, 8, productRows);
        productWindow.Draw();
        Console.WriteLine("[V] för att välja ID [B] För att gå tillbaks till Kategorier");
    }

    public static void DrawProductDetails(MyDbContext db, int productId)
    {
        var product = db.Products.Include(p => p.ProductItems).Include(p => p.ProductGenres)
            .ThenInclude(pg => pg.Genres).FirstOrDefault(p => p.Id == productId);

        var item = product.ProductItems.FirstOrDefault();
        bool canBuy = item.UnitsInStock > 0;
        List<string> itemDetails = new List<string>
        {
            $"Titel: {product.Title}",
            $"Beskrivning: {product.Description}",
            $"Lager: {item?.UnitsInStock}",
            $"Lager: {item?.Price}",
            $"Skick: {item?.Condition}",
            canBuy ? "[K] KÖP PRODUKT" : "SLUT PÅ LAGRET"
        };

        var detailWindow = new UX.Window($"{product.Title}", 50, 12, itemDetails);
        detailWindow.Draw();
    }



}