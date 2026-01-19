namespace Spelshoppen.Transactions;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }

    public string Street { get; set; }
    public string ZipCode { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public int CityId { get; set; }
    public City City { get; set; }

    public int PaymentId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public List<OrderLine> OrderLines { get; set; }
    
}