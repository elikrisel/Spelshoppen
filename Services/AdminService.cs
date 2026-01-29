using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class AdminService
{
    public static void AddProduct(MyDbContext db, string title, string description, decimal price, int stock, int categoryId,
        List<int> genreIds,int suppId, string condition)
    {
        var newProduct = new Product
        {
            Title = title,
            Description = description,
            CategoryId = categoryId,
            ProductGenres = new List<ProductGenre>()
        };

        if (genreIds != null && genreIds.Count > 0)
        {
            foreach (var genreId in genreIds)
            {
                newProduct.ProductGenres.Add(new ProductGenre 
                { 
                    GenreId = genreId 
                });
            }
        }
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
    public static bool DeleteProduct(MyDbContext db, int productId)
    {
        //Inkluderar produkt och sen inkluderar Product Genre för att ta bort produkten från genres.
        var item = db.ProductItems.Include(p => p.Products).
            ThenInclude(product => product!.ProductGenres).
            FirstOrDefault(p => p.ProductId == productId);
        
        //Tar bort både från Products och ProductItem
        if (item != null && item.Products != null)
        {
            db.ProductGenres.RemoveRange(item.Products.ProductGenres);   
            db.ProductItems.Remove(item);
            db.Products.Remove(item.Products);
            db.SaveChanges();
        }
        return true;
    }
    
}