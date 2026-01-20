using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class WindowExample
{
    public static void DrawShop()
    {
        
        List<string> topText2 = new List<string> { "Tröja", "Fin tröja i ull", "Pris: 149 kr", "Tryck A för att köpa" };
        var windowTop2 = new Window("Erbjudande 1", 10, 12, topText2);
        windowTop2.Draw();

        List<string> topText3 = new List<string> { "Byxor", "Lagom långa byxor", "Pris: 299 kr", "Tryck B för att köpa" };
        var windowTop3 = new Window("Erbjudande 2",36, 12, topText3);
        windowTop3.Draw();

        List<string> topText4 = new List<string> { "Läderskor", "Extra flotta", "Pris: 450 kr", "Tryck C för att köpa" };
        var windowTop4 = new Window("Erbjudande 3", 64, 12, topText4);
        windowTop4.Draw();


    }
    
    public static void DrawCategoryMenu(MyDbContext db)
    {
        var categories = db.Categories.ToList();
        List<string> categoryRows = new List<string>();

        for (int i = 0; i < categories.Count; i++)
        {
            categoryRows.Add($"[{i + 1}] {categories[i].Title}");
        }
        
        var categoryWindow = new UX.Window("Kategorier:",35,8,categoryRows);
        categoryWindow.Draw();

    }

    public static void DrawProductMenu(MyDbContext db, int selectedId)
    {
        if (selectedId <= 0) return;
        
        var categoryName = db.Categories.FirstOrDefault(c => c.Id == selectedId)?.Title;
        
        var products = db.Products.Where(p => p.CategoryId == selectedId).ToList();
        List<string> productRows = products.Select(p => $"{p.Id}: {p.Title}").ToList();
        
        var productWindow = new UX.Window($"{categoryName}",40,8,productRows);
        productWindow.Draw();
    }
    
    
    public static Window FeaturedWindow(ProductItem item, int left, int top, int index)
    {
        string title = item.Products?.Title ?? "Okänd produkt";
        string condition = item.Condition ?? "Ny";

        List<string> content = new List<string>
        {   title,
            $"Skick: {condition}",
            $"Pris: {item.Price}",
            $"Lager: {item.UnitsInStock}",
            $"Tryck {index} för att köpa"
        };
        return new Window($"Produkt {index}", left, top, content);

    }
    
    
    
}
