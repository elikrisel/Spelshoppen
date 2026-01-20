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
    }

    public static void DrawProductDetails(MyDbContext db, int productId)
    {
        var product = db.Products.Include(p => p.ProductItems).Include(p => p.ProductGenres)
            .ThenInclude(pg => pg.Genres).FirstOrDefault(p => p.Id == productId);

        var item = product.ProductItems.FirstOrDefault();
        List<string> itemDetails = new List<string>
        {
            $"Titel: {product.Title}",
            $"Beskrivning: {product.Description}",
            $"Lager: {item?.UnitsInStock}",
            $"Lager: {item?.Price}",
            $"Skick: {item?.Condition}"
        };

        var detailWindow = new UX.Window($"{product.Title}", 50, 12, itemDetails);
        detailWindow.Draw();
    }


    public static Window FeaturedWindow(ProductItem item, int left, int top, int index)
    {
        string title = item.Products?.Title ?? "Okänd produkt";
        string condition = item.Condition ?? "Ny";

        List<string> content = new List<string>
        {
            title,
            $"Skick: {condition}",
            $"Pris: {item.Price}",
            $"Lager: {item.UnitsInStock}",
            $"Tryck {index} för att köpa"
        };
        return new Window($"Produkt {index}", left, top, content);
    }
}