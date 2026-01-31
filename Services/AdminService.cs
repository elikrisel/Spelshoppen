using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class AdminService
{
    public static void AddProduct(MyDbContext db, string title, string description, decimal price, int stock, int categoryId,
        List<int> genreIds,int suppId, string condition)
    {   //Lägger till en ny product
        var newProduct = new Product
        {
            Title = title,
            Description = description,
            CategoryId = categoryId,
            ProductGenres = new List<ProductGenre>()
        };
        //Om det är spel så läggs genres in
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
        //Lägger in en product item
        var newItem = new ProductItem
        {
            Products = newProduct,
            SupplierId = suppId,
            Price = price,
            UnitsInStock = stock,
            Condition = condition,
            IsFeatured = false,
        };
        //Lägger in och sparar
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
        if (item == null || item.Products == null) return false;
        try
        {
            db.ProductGenres.RemoveRange(item.Products.ProductGenres);   
            db.ProductItems.Remove(item);
            db.Products.Remove(item.Products);
            db.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
        
    }
    //Skapar ny Kategori och sparar i databasen
    public static void AddNewCategory(MyDbContext db, string title)
    {
        var newCategory = new Category {Title = title };
        db.Categories.Add(newCategory);
        db.SaveChanges();
    }
    
}