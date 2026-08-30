using Core.Model;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=ProductsP422;User Id=sa;Password=Test4ServerPasw0rd;TrustServerCertificate=True;Encrypt=True");
    }
}
