using Microsoft.EntityFrameworkCore;
using OdezdaShopServerApp.Core.Entities;
using OdezdaShopServerApp.Core.Interfaces.Repositories;
using OdezdaShopServerApp.Data;

namespace OdezdaShopServerApp.Core.Repositories
{
    public class EfOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public EfOrderRepository(ApplicationDbContext db) => _db = db;


        public async Task AddAsync(Order order) => await _db.Orders.AddAsync(order);

        public async Task<IEnumerable<Order>> GetAllAsync(string id)
        {
            return await _db.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == id)
                .ToListAsync();
        }

        public async Task<Order?> GetAsync(Guid id) => await _db.Orders.FindAsync(id);
        public Task SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
