using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Mini_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Mini_Project.Configuration
{
    internal sealed class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProducts");
            builder.HasKey(op => new { op.OrderId, op.ProductId });
            builder.Property(op => op.Quantity).IsRequired();
            builder.Property(op => op.PricePerItem).IsRequired();
            builder.HasOne(op => op.Order).WithMany(o => o.OrderProducts).HasForeignKey(op => op.OrderId);
            builder.HasOne(op => op.Product).WithMany(p => p.OrderProducts).HasForeignKey(op => op.ProductId);
            builder.HasCheckConstraint("CK_OrderProduct_Quantity_NonNegative", "[Quantity] >= 0");
            builder.HasCheckConstraint("CK_OrderProduct_PricePerItem_NonNegative", "[PricePerItem] >= 0");
        }
    }
}

