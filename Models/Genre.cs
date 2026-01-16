namespace Spelshoppen.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<ProductGenre> ProductGenres { get; set; } = new List<ProductGenre>();
}