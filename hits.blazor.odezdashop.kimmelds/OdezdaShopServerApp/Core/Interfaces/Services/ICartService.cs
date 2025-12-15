using OdezdaShopServerApp.Core.Entities.Carts;

namespace OdezdaShopServerApp.Core.Interfaces.Services
{
    public interface ICartService
    {
        Cart GetCart();
        void AddToCart(Guid productId, string productName, decimal unitPrice, int quantity = 1);
        void RemoveFromCart(Guid productId);
        void UpdateQuantity(Guid productId, int quantity);
        void ClearCart();
        event Action? OnChange;
    }
}
