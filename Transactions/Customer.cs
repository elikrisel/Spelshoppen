using Spelshoppen.Models;

namespace Spelshoppen.Transactions;

public class Customer
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? Street { get; set; }
    
    public string? EmailAddress { get; set; }
    public string? PhoneAddress { get; set; }
    
    public DateTime Age { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}