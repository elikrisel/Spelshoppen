using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class StoreServices
{
    
// Hämtar allt för detaljvyn: Produkt, dess Items (lager/pris) och dess Genrer
    public static Product? GetFullProduct(MyDbContext db, int productId)
    {
        return db.Products
            .Include(p => p.ProductItems)
            .ThenInclude(pi => pi.Suppliers)
            .Include(p => p.ProductGenres)
            .ThenInclude(pg => pg.Genres)
            .FirstOrDefault(p => p.Id == productId);
    }

    public static ProductItem? GetPurchaseableItem(MyDbContext db, int productId)
    {
        // Vi letar efter ett item för denna produkt som faktiskt finns i lager
        return db.ProductItems
            .Include(pi => pi.Products)
            .FirstOrDefault(pi => pi.ProductId == productId && pi.UnitsInStock > 0);
    }
    
}