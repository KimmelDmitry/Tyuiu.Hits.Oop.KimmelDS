using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OdezdaShopServerApp.Core.Entities;

namespace OdezdaShopServerApp.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(oi => oi.Id);

        // Явное свойство OrderId уже есть в классе
        builder.Property(oi => oi.OrderId).IsRequired();

        builder.Property(oi => oi.ProductId).IsRequired();
        builder.Property(oi => oi.ProductName).IsRequired().HasMaxLength(300);
        builder.Property(oi => oi.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(oi => oi.Quantity).IsRequired();
    }
}

