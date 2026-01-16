namespace Spelshoppen.Models;

public class Product
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public int CategoryId { get; set; }
    public virtual Category Categories { get; set; }
    
    //Många genres
    public virtual ICollection<ProductGenre> ProductGenres { get; set; } = new List<ProductGenre>();
    
    // Ny/Begagnad
    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
    
}