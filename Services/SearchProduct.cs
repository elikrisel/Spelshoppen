using Microsoft.Data.SqlClient;
using Dapper;
using Spelshoppen.Models;

namespace Spelshoppen;

public class SearchProduct
{
    private static string connString =
        "Server=.\\SQLExpress;Database=Spelshoppen;Trusted_Connection=True; TrustServerCertificate=True;";
    
    public static List<ProductSearchResult> SearchProducts(string searchTerm)
    {
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
        List<Models.ProductSearchResult> allSearchResults = new List<Models.ProductSearchResult>();
        using var connection = new SqlConnection(connString);
        allSearchResults = connection.Query<Models.ProductSearchResult>
            (sql, new { SearchTerm = searchTerm }).ToList();

        return allSearchResults;

    }
}