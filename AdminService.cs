using Spelshoppen.Models;

namespace Spelshoppen;

public class AdminService
{
    public static void AddProduct(MyDbContext db, string title, string desc, decimal price, int stock, int catId,
        int suppId, string condition)
    {
        var newProduct = new Product
        {
            Title = title,
            Description = desc,
            CategoryId = catId,
        };
        var newItem = new ProductItem
        {
            Products = newProduct,
            SupplierId = suppId,
            Price = price,
            UnitsInStock = stock,
            Condition = condition,
            IsFeatured = false,
        };

        db.ProductItems.Add(newItem);
        db.SaveChanges();

    }
}