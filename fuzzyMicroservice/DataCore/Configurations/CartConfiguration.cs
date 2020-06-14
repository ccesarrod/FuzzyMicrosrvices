using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Entity.ModelConfiguration;

namespace DataCore.Configurations
{
    public class CartConfiguration: IEntityTypeConfiguration<CartDetails>
    {
        public CartConfiguration()
        {
           
        }

        public void Configure(EntityTypeBuilder<CartDetails> modelBuilder)
        {
            modelBuilder.ToTable("Cart");
            modelBuilder.HasKey(t => t.Id);

           
           
        }
    }
}
