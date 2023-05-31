using Caching.InMemory.Model;
using Microsoft.EntityFrameworkCore;

namespace Caching.InMemory.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product[]
            {
                new Product
                {
                    ProductId = 1,
                    ProductDescription = "Monitor Samsung 17 Inch",
                    ProductName = "Monitor",
                    Stock= 10,
                },
                new Product
                {
                    ProductId = 2,
                    ProductDescription = "Monitor Samsung 15 Inch",
                    ProductName = "Monitor",
                    Stock= 10,
                },
                new Product
                {
                    ProductId = 3,
                    ProductDescription = "Monitor Samsung 27 Inch",
                    ProductName = "Monitor",
                    Stock= 10,
                },
            });
        }
    }
}
