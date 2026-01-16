using Microsoft.EntityFrameworkCore;

namespace Spelshoppen.Models;

public class MyDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductItem> ProductItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductGenre> ProductGenres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Spelshoppen");
        //GenreId och ProductId tillsammans är PK
        modelBuilder.Entity<ProductGenre>().HasKey(pg => new { pg.GenreId, pg.ProductId });
        //Sätter precision för priset
        modelBuilder.Entity<ProductItem>().Property(p => p.Price).HasPrecision(18, 2);
        
    }
    
    //TODO: Testing purposes only
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Spelshoppen;Trusted_Connection=True; TrustServerCertificate=True;");
    }
    
}