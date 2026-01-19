using Spelshoppen.Models;

namespace Spelshoppen.Transactions;

public class OrderLine
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductItemId { get; set; }
    public ProductItem ProductItem { get; set; }

    public int Quantity { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal VatRate { get; set; }
}