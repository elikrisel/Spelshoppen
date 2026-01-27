using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class StoreServices
{
    
    //Hämtar all information om Produkten
    public static Product? GetFullProduct(MyDbContext db, int productId) =>
        db.Products
            .Include(p => p.ProductItems)
            .ThenInclude(pi => pi.Suppliers)
            .Include(p => p.ProductGenres)
            .ThenInclude(pg => pg.Genres)
            .FirstOrDefault(p => p.Id == productId);

    // Vi letar efter ett item för denna produkt som faktiskt finns i lager
    public static ProductItem? GetPurchasableItem(MyDbContext db, int productId) =>
        db.ProductItems
            .Include(pi => pi.Products)
            .FirstOrDefault(pi => pi.ProductId == productId && pi.UnitsInStock > 0);
    
    
    //Hämtar tre stycken produkter som har "IsFeatured" till Erbjudanden
    public static List<ProductItem> GetFeaturedItems(MyDbContext db) =>
        db.ProductItems.Include(pi => pi.Products)
            .Where(pi => pi.IsFeatured && pi.UnitsInStock > 0).Take(3).ToList();

    public static void ExecutePurchase(MyDbContext db, UserSession session, ProductItem item)
    {
        if (item != null)
        {
            
            item.UnitsInStock--;
        
            //Kolla om varan redan finns i Dictionary
            var existingKey = session.CartItem.Keys.FirstOrDefault(k => k.Id == item.Id);
            
            //Kollar om varan finns i varukorgen eller inte
            if (existingKey != null)
            {
                
                session.CartItem[existingKey]++;
            }
            else
            {
                
                session.CartItem.Add(item, 1);
            }

            session.NotificationMessage = $"{item.Products?.Title} tillagd i korgen!";
             
        }
        else
        {
            session.NotificationMessage = "Varan är tyvärr slut i lager.";
            
        }

    }
    
}