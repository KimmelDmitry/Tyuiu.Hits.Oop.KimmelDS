using OdezdaShopServerApp.Core.Entities;

namespace OdezdaShopServerApp.Core.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetAsync(Guid id);
    Task<List<Category>> ListAsync();
    Task AddAsync(Category category);
    Task SaveChangesAsync();
}
