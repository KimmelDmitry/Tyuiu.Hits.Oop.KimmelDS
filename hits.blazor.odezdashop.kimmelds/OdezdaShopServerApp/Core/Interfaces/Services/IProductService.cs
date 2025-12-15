using OdezdaShopServerApp.Core.Entities;

namespace OdezdaShopServerApp.Core.Interfaces.Services;

public interface IProductService
{
    Task<Product> CreateAsync(string name, decimal price, Guid categoryId, int stock, string? description, string? imageFileName, byte[]? imageBytes);
    Task UpdateAsync(Product product, string? imageFileName = null, byte[]? imageBytes = null);
    Task<List<Product>> ListActiveAsync();
    Task<Product?> GetAsync(Guid id);
    Task DeleteAsync(Guid id);

    Task<List<Product>> GetAllAsync();
}
