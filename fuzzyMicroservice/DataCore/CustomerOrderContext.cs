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
            modelBuilder.Entity<CartDetails>().ToTable("Cart");

            base.OnModelCreating(modelBuilder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        //.UseSqlServer(this.Database.GetDbConnection().ConnectionString);
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
