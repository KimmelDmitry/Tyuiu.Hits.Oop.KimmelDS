using OdezdaShopServerApp.Core.Entities;

namespace OdezdaShopServerApp.Core.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetAsync(Guid id);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> ListActiveAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task SaveChangesAsync();
}
