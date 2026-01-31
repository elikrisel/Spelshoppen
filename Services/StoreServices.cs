using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class StoreServices
{
    //Hämtar all information om Produkten och går igenom all relaterad data som ligger i andra tabeller
    public static Product? GetFullProduct(MyDbContext db, int productId) =>
        db.Products
            .Include(p => p.ProductItems)
            .ThenInclude(pi => pi.Suppliers)
            .Include(p => p.ProductGenres)
            .ThenInclude(pg => pg.Genres)
            .FirstOrDefault(p => p.Id == productId);
    
    // Vi letar efter ett item beroende på id input och kollar om produkten finns i lagret
    public static ProductItem? GetPurchasableItem(MyDbContext db, int productId) =>
        db.ProductItems
            .Include(pi => pi.Products)
            .FirstOrDefault(pi => pi.ProductId == productId && pi.UnitsInStock > 0);
    
    //Hämtar tre stycken produkter som har "IsFeatured" till Erbjudanden. Extra säkerhetsåtgärd fastän man har ställt in hur många som visas
    public static List<ProductItem> GetFeaturedItems(MyDbContext db) =>
        db.ProductItems.Include(pi => pi.Products)
            .Where(pi => pi.IsFeatured && pi.UnitsInStock > 0).Take(3).ToList();
    
    //Togglar vilket item som ska vara i featured. Om redan tre stycken är featured, så returnar den
    public static bool ToggleFeaturedStatus(MyDbContext db, int productId)
    {
        //Letar enbart efter den unika nyckeln för att ändra på IsFeatured
        var item = db.ProductItems.Find(productId);

        if (!item.IsFeatured)
        {
            //Kollar hur många items som är satt på IsFeatured
            int currentFeaturedCount = db.ProductItems.Count(pi => pi.IsFeatured);

            if (currentFeaturedCount >= 3) return false;
        }

        item.IsFeatured = !item.IsFeatured;
        db.SaveChanges();

        return true;
    }

    public static void ExecutePurchase(MyDbContext db, UserSession session, ProductItem item)
    {
        if (item != null)
        {   //Tar från lagret
            item.UnitsInStock--;

            //Kolla om varan redan finns i Dictionary
            var existingKey = session.CartItem.Keys.FirstOrDefault(k => k.Id == item.Id);

            //Kollar om varan finns i varukorgen eller inte och uppdaterar
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