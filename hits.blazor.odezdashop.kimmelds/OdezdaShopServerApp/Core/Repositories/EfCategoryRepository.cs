using Microsoft.EntityFrameworkCore;
using OdezdaShopServerApp.Core.Entities;
using OdezdaShopServerApp.Core.Interfaces.Repositories;
using OdezdaShopServerApp.Data;

namespace OdezdaShopServerApp.Core.Repositories
{
    public class EfCategoryRepository(ApplicationDbContext _db) : ICategoryRepository
    {
        public async Task AddAsync(Category category) => await _db.Categories.AddAsync(category);
        public async Task<Category?> GetAsync(Guid id) => await _db.Categories.FindAsync(id);
        public async Task<List<Category>> ListAsync() => await _db.Categories.ToListAsync();
        public Task SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
