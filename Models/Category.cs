namespace Spelshoppen.Models;

public class Category
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    
    //En kategori innehåller många produkter
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}