using Microsoft.EntityFrameworkCore;
using OdezdaShopServerApp.Core.Entities;
using OdezdaShopServerApp.Core.Interfaces.Repositories;
using OdezdaShopServerApp.Data;

namespace OdezdaShopServerApp.Core.Repositories;

public class EfProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;


    public EfProductRepository(ApplicationDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }


    public async Task AddAsync(Product product)
    {
        if (product == null) 
            throw new ArgumentNullException(nameof(product));
        
        await _db.Products.AddAsync(product);
    }


    public async Task<Product?> GetAsync(Guid id)
    {
        if (id == Guid.Empty) 
            return null;
        // Include Category if needed in callers
        return await _db.Products.FindAsync(id);
    }


    public async Task<List<Product>> ListActiveAsync()
    {
        return await _db.Products
            .Where(p => p.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Product>> GetAllAsync() => await _db.Products.AsNoTracking().ToListAsync();

    public async Task UpdateAsync(Product product)
    {
        await Task.Run(() =>
        {
            if (product == null) 
                throw new ArgumentNullException(nameof(product));

            _db.Products.Update(product);
        });
    }


    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    
}
