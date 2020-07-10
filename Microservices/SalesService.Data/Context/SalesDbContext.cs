using SalesService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SalesService.Data.Context
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options)
            : base(options)
        {
        }
        public DbSet<ShoppingBasketItem> ShoppingBasketItems { get; set; }

        public DbSet<ShoppingBasketProduct> ShoppingBasketProducts {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingBasketItem>()
                .HasKey(c => new { c.ShoppingBasketID, c.ProductID });

            modelBuilder.Entity<ShoppingBasketProduct>()
                .HasIndex(b => b.ProductId);
        }
    }
}
