using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Spelshoppen.Models;

namespace Spelshoppen;

public class SearchProduct
{
    //SEARCH FUNKTION VIA DAPPER
    public static List<ProductSearchResult> SearchProducts(string searchTerm, MyDbContext db)
    {
        var connection = db.Database.GetDbConnection();
        //Har ändrat namn på databasen så jag behöver kalla på schemaName för att den ska hitta tables
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