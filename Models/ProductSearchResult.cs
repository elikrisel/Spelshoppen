namespace Spelshoppen.Models;

//Search Result klass för att utnyttja Dapper
public class ProductSearchResult
{
    public int ProductItemId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}