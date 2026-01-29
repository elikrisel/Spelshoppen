namespace Spelshoppen.Transactions;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }

    public string? Street { get; set; }
    public string? ZipCode { get; set; }

    public int CustomerId { get; set; }
    public virtual Customer Customers { get; set; }

    public int CityId { get; set; }
    public virtual City Cities { get; set; }

    public int PaymentId { get; set; }
    public virtual PaymentMethod? PaymentMethods { get; set; }

    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    
}