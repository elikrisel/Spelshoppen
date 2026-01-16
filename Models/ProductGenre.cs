namespace Spelshoppen.Models;

public class ProductGenre
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    public virtual Product Products { get; set; }

    public int GenreId { get; set; }
    public virtual Genre Genres { get; set; }
}