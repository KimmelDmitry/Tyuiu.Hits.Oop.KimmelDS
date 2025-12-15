using OdezdaShopServerApp.Core.Entities;

namespace OdezdaShopServerApp.Core.Interfaces.Services;

public interface IOrderService
{
    public Task<Order> PlaceOrderAsync((Guid productId, int qty)[] items, string? userId, string? shippingAddress, string? email);
    public Task<Order?> GetAsync(Guid id);

    public Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);

}
