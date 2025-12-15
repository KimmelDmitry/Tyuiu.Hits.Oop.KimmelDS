using OdezdaShopServerApp.Core.Entities;

namespace OdezdaShopServerApp.Core.Interfaces.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync(string id);
    Task SaveChangesAsync();
}
