using Microsoft.EntityFrameworkCore;
using OdezdaShopServerApp.Core.Entities;
using OdezdaShopServerApp.Data;
using OdezdaShopServerApp.VerticalSlices.Dto;
using System;

namespace OdezdaShopServerApp.VerticalSlices
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = c.Products.Count
                })
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _db.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (entity == null) return null;

            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                ProductCount = entity.Products.Count
            };
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto, CancellationToken cancellationToken = default)
        {
            var exist = await _db.Categories.Where(c => c.Name == dto.Name.Trim()).AnyAsync(cancellationToken);

            if (exist) return null;

            var entity = new Category(dto.Name.Trim());
            _db.Categories.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                ProductCount = 0
            };
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, CategoryUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _db.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (entity == null) throw new KeyNotFoundException($"Category {id} not found.");

            entity.Rename(dto.Name.Trim());
            await _db.SaveChangesAsync(cancellationToken);

            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                ProductCount = entity.Products.Count
            };
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _db.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (entity == null) throw new KeyNotFoundException($"Category {id} not found.");

            if (entity.Products != null && entity.Products.Any())
                throw new InvalidOperationException("Cannot delete category that has products. Remove or move products first.");

            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _db.Categories.AnyAsync(c => c.Id == id, cancellationToken);
        }
    }
}
