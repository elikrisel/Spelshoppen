namespace Spelshoppen.Transactions;

public class PaymentMethod
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public List<Order> Orders { get; set; }

}