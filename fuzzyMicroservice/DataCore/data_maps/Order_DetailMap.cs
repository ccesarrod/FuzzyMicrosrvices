using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataCore.data_maps
{
    public class Order_DetailMap : IEntityTypeConfiguration<OrderDetail>
    {
          public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            // Primary Key
            builder.HasKey(t => new { t.OrderID, t.ProductID });

            // Properties
            //builder.Property(t => t.OrderID)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //builder.Property(t => t.ProductID)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            builder.ToTable("Order Details");
            //builder.Property(t => t.OrderID).HasColumnName("OrderID");
            //builder.Property(t => t.ProductID).HasColumnName("ProductID");
            //builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            //builder.Property(t => t.Quantity).HasColumnName("Quantity");
            //builder.Property(t => t.Discount).HasColumnName("Discount");

            // Relationships

            builder.HasOne(t => t.Order)
                .WithMany(t => t.Order_Details)
                .HasForeignKey(d => d.OrderID);
        }
    }
}
