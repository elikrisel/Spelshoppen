using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Spelshoppen.Transactions;

namespace Spelshoppen.Models;

public class MyDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductItem> ProductItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductGenre> ProductGenres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    
    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Spelshoppen");
        //GenreId och ProductId tillsammans är PK
        modelBuilder.Entity<ProductGenre>().HasKey(pg => new { pg.GenreId, pg.ProductId });
        //Sätter precision för priset
        modelBuilder.Entity<ProductItem>().Property(p => p.Price).HasPrecision(18, 2);
        
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connString = config["MySettings:ConnectionString"];
        optionsBuilder.UseSqlServer(connString);
        
    }
    
}