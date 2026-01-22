using Spelshoppen.Models;

namespace Spelshoppen.UX;

public class CategoryMenu
{
    public static void DrawCategoryMenu(MyDbContext db, int categoryId, int productId)
    {
        var categories = db.Categories.Select(c => $"[{c.Id}] {c.Title}").ToList();
        new UX.Window("Kategorier", 20, 8, categories).Draw();
        if(categoryId != 0)
        
        //Om en kategori är vald
        if (categoryId != 0)
        {
            var products = db.Products.Where(p => p.CategoryId == categoryId)
                .Select(p => $"[{p.Id}] {p.Title}").ToList();
            new UX.Window("Produkter", 40, 8, products).Draw();
        }
        
        //Detaljer
        if (productId != 0)
        {
            var product = StoreServices.GetFullProduct(db,productId);
            if (product != null)
            {
                var item = product.ProductItems.FirstOrDefault();
                var genres = string.Join(", ", product.ProductGenres.Select(pg => pg.Genres.Name));
                
                var productDetails = new List<string>
                {
                    $"Titel:  {product.Title}",
                    $"Beskrivning: {product.Description}",
                    $"Genre:  {genres}",
                    $"Pris:   {item?.Price} kr",
                    $"Skick:  {item?.Condition}",
                    $"Lager:  {item?.UnitsInStock} st",
                    "",
                    item?.UnitsInStock > 0 ? "[ENTER] LÄGG I KORG" : "SLUT I LAGER"
                };
                new UX.Window("PRODUKTINFORMATION", 70, 8, productDetails).Draw();
            }
        }
        
        Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
        
    
        if (categoryId == 0)
            Console.Write(" Välj Kategori genom att skriva ID: ");
        else if (productId == 0)
            Console.Write(" Välj Produkt genom att skriva ID: ");
        else
            Console.Write(" Skriv ett annat produkt-ID eller tryck [ENTER] för att köpa: ");

        Console.ResetColor();
        
    }
}