using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class SearchProduct
{
    
    public static List<ProductSearchResult> SearchProducts(string searchTerm, MyDbContext db)
    {
        var connection = db.Database.GetDbConnection();
        string schemaName = "Spelshoppen";
        string sql = $"""
                      SELECT 
                          pi.Id AS ProductItemId, 
                          p.Title,
                          pi.Price
                      FROM {schemaName}.ProductItems pi
                      JOIN {schemaName}.Products p ON pi.ProductId = p.Id
                      WHERE p.Title LIKE '%' + @SearchTerm + '%'
                         OR p.Description LIKE '%' + @SearchTerm + '%'
                      """;
        
        
        return connection.Query<Models.ProductSearchResult>(sql, new { SearchTerm = searchTerm }).ToList();
        
        
    }
}