using OdezdaShopServerApp.Core.Entities.Carts;
using OdezdaShopServerApp.Core.Interfaces.Services;

namespace OdezdaShopServerApp.Core.Services;

public class InMemoryCartService : ICartService
{
    private readonly Cart _cart = new();

    public event Action? OnChange;

    public Cart GetCart() => _cart;

    public void AddToCart(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        var existing = _cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing == null)
        {
            _cart.Items.Add(new CartItem
            {
                ProductId = productId,
                ProductName = productName,
                UnitPrice = unitPrice,
                Quantity = quantity
            });
        }
        else
        {
            existing.Quantity = quantity;
        }
        OnChange?.Invoke();
    }

    public void RemoveFromCart(Guid productId)
    {
        var item = _cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null) _cart.Items.Remove(item);
        OnChange?.Invoke();
    }

    public void UpdateQuantity(Guid productId, int quantity)
    {
        var item = _cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return;
        if (quantity <= 0) _cart.Items.Remove(item);
        else item.Quantity = quantity;
        OnChange?.Invoke();
    }

    public void ClearCart()
    {
        _cart.Items.Clear();
        OnChange?.Invoke();
    }
}
