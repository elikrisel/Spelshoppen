namespace Spelshoppen.Models;

public class ProductItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public virtual Product Products { get; set; }

    public int? SupplierId { get; set; }
    public virtual Supplier Suppliers { get; set; }

    public decimal Price { get; set; }
    public int UnitsInStock { get; set; }

    public bool IsFeatured { get; set; }
    public string? Condition { get; set; }
    
}