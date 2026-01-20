using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class StoreServices
{
    public static Product? PurchaseProduct(MyDbContext db, int productId)
    {
        // Viktigt: 'Products' måste matcha namnet i din ProductItem-klass!
        var item = db.ProductItems
            .Include(p => p.Products) 
            .FirstOrDefault(p => p.ProductId == productId);

        if (item != null && item.UnitsInStock > 0)
        {
            item.UnitsInStock--;
            // db.SaveChanges(); // Kommenterad för test
            return item.Products; 
        }
        return null;
    }

    
    
    
}