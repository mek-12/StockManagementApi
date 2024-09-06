using Microsoft.EntityFrameworkCore;
using StockManagement.DAL.Core.Entities;

namespace StockManagement.DAL.Concrete {
    public class StockManagementDbContext : DbContext {
        public StockManagementDbContext(DbContextOptions<StockManagementDbContext> options)
           : base(options) {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.PriceHistories)
                .WithOne(ph => ph.Product)
                .HasForeignKey(ph => ph.ProductId);

        }
    }
}
