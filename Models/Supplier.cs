namespace Spelshoppen.Models;

public class Supplier
{
    public int Id { get; set; }
    public string? PublisherName { get; set; }
    public string? ContactName { get; set; }
    public string? ContactInformation { get; set; }

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}