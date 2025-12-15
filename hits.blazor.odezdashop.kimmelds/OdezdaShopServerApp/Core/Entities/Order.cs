namespace OdezdaShopServerApp.Core.Entities;

public enum OrderStatus { Pending = 0, Paid = 1, Shipped = 2, Completed = 3, Cancelled = 4 }


public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OrderNumber { get; set; } = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 6)}";
    public string? UserId { get; set; } // IdentityUser.Id
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;


    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();


    public decimal Total => _items.Sum(x => x.Total);


    public string? ShippingAddress { get; set; }
    public string? Email { get; set; }


    public Order(string? userId = null, string? shippingAddress = null, string? email = null)
    {
        UserId = userId;
        ShippingAddress = shippingAddress;
        Email = email;
    }


    private Order() { } // EF


    public void AddItem(OrderItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        // привязываем навигацию и FK к родительскому заказу
        item.Order = this;
        item.OrderId = this.Id;
        _items.Add(item);
    }



    public void SetStatus(OrderStatus status) => Status = status;
}
