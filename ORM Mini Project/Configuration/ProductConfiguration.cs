using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Mini_Project.Models;
using System;
namespace ORM_Mini_Project.Configuration
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Stock).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.UpdatedDate).IsRequired();
            builder.HasCheckConstraint("CK_Product_Price_NonNegative", "[Price] >= 0");
            builder.HasCheckConstraint("CK_Product_Stock_NonNegative", "[Stock] >= 0");
        }
    }
}
