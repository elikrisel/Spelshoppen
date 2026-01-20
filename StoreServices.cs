using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class StoreServices
{
    public static Product? PurchaseProduct(MyDbContext db, int productId)
    {
        var item = db.ProductItems.Include(p => p.Products)
            .FirstOrDefault(p => p.ProductId == productId);
        if (item != null && item.UnitsInStock > 0)
        {
            item.UnitsInStock--;
            //db.SaveChanges();
            return item.Products;
        }
        
        return null;
        
    }

    
    
    
}