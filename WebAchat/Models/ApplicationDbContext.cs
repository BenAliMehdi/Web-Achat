using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAchat.Models;

namespace WebAchat.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<TempBuyOrder> TempBuyOrders { get; set; }
        public DbSet<TempOrderProduct> TempOrderProducts { get; set; }
        public DbSet<TempProduct> TempProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TempBuyOrder>()
                .HasMany(t => t.OrderProducts)
                .WithOne(op => op.TempBuyOrder)
                .HasForeignKey(op => op.TempBuyOrderId);

            modelBuilder.Entity<TempOrderProduct>()
                .HasOne(op => op.TempProduct)
                .WithMany()
                .HasForeignKey(op => op.TempProductId);
        }
    }
}