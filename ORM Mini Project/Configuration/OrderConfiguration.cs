using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Models.Enums;

namespace ORM_Mini_Project.Configuration
{
    internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.UserId).IsRequired();
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.TotalAmount).IsRequired();
            builder.Property(o => o.Status).IsRequired().HasConversion(v => v.ToString(), v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));
            builder.HasCheckConstraint("CK_Order_TotalAmount_NonNegative", "[TotalAmount] >= 0");
        }
    }
}
