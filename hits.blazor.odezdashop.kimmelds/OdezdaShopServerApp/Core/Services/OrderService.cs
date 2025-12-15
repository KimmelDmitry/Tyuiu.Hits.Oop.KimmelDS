using OdezdaShopServerApp.Core.Entities;
using OdezdaShopServerApp.Core.Interfaces.Repositories;
using OdezdaShopServerApp.Core.Interfaces.Services;
using OdezdaShopServerApp.Data;

namespace OdezdaShopServerApp.Core.Services;

public class OrderService : IOrderService
{
    private readonly IProductRepository _productRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly ApplicationDbContext _db;


    public OrderService(IProductRepository productRepo, IOrderRepository orderRepo, ApplicationDbContext db)
    {
        _productRepo = productRepo;
        _orderRepo = orderRepo;
        _db = db;
    }



    public async Task<Order> PlaceOrderAsync((Guid productId, int qty)[] items, string? userId, string? shippingAddress, string? email)
    {
        if (items == null || items.Length == 0) throw new ArgumentException("Cart empty");

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var order = new Order(userId, shippingAddress, email);

            foreach (var (productId, qty) in items)
            {
                var product = await _productRepo.GetAsync(productId)
                    ?? throw new InvalidOperationException($"Product {productId} not found");

                if (!product.IsActive) throw new InvalidOperationException("Product not available");
                if (product.Stock < qty) throw new InvalidOperationException("Insufficient stock for product " + product.Name);

                product.AdjustStock(-qty);

                // помечаем продукт как изменённый в текущем контексте
                _db.Products.Update(product);

                var item = new OrderItem(product.Id, product.Name, product.Price, qty);
                order.AddItem(item);
            }

            // Добавляем заказ в контекст
            _db.Orders.Add(order);

            // Один SaveChanges — атомарно вставит Orders и OrderItems, и обновит Products
            await _db.SaveChangesAsync();

            await tx.CommitAsync();
            return order;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }



    public async Task<Order?> GetAsync(Guid id) => await _orderRepo.GetAsync(id);

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
    {
        return await _orderRepo.GetAllAsync(userId);
    }
}
