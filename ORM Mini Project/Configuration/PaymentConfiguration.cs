using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Configuration
{
    internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.OrderId).IsRequired();
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.PaymentDate).IsRequired();
            builder.HasOne(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
            builder.HasCheckConstraint("CK_Payment_Amount_NonNegative", "[Amount] >= 0");
        }
    }
}
