using DataCore.Configurations;
using DataCore.data_maps;
using DataCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataCore
{
    public class CustomerOrderContext : DbContext
    {
        public CustomerOrderContext() { }
        public CustomerOrderContext(DbContextOptions<CustomerOrderContext> options) : base(options) { }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Customer>()
                .HasMany(t => t.Cart)
                .WithOne(c => c.Customer)
                .HasForeignKey(c => c.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<CartDetails>().HasKey(t => t.Id);
            //modelBuilder.Entity<CartDetails>().ToTable("Cart");
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.Entity<Order>().ToTable("Orders");         
            modelBuilder.ApplyConfiguration(new Order_DetailMap());
            base.OnModelCreating(modelBuilder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
