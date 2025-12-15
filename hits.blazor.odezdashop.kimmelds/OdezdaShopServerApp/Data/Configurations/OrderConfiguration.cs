using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OdezdaShopServerApp.Core.Entities;

namespace OdezdaShopServerApp.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(100);
        builder.Property(o => o.UserId).HasMaxLength(450);
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.Status).HasConversion<int>().IsRequired();
        builder.Property(o => o.ShippingAddress).HasMaxLength(1000);
        builder.Property(o => o.Email).HasMaxLength(320);

        builder.Ignore(o => o.Total);

        // Явно указываем навигацию и FK
        builder.HasMany(o => o.Items)
               .WithOne(i => i.Order)
               .HasForeignKey(i => i.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}

