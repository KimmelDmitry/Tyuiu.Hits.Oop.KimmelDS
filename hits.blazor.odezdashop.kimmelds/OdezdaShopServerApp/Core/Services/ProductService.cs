using OdezdaShopServerApp.Core.Entities;
using OdezdaShopServerApp.Core.Interfaces.Repositories;
using OdezdaShopServerApp.Core.Interfaces.Services;

namespace OdezdaShopServerApp.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IImageStorageService _imageStorage;


        public ProductService(IProductRepository productRepo, IImageStorageService imageStorage)
        {
            _productRepo = productRepo;
            _imageStorage = imageStorage;
        }


        public async Task<Product> CreateAsync(string name, decimal price, Guid categoryId, int stock, string? description, string? imageFileName, byte[]? imageBytes)
        {
            string? path = null;
            
            if (imageFileName != null && imageBytes != null && imageBytes.Length > 0)
            {
                path = await _imageStorage.SaveProductImageAsync(imageFileName, imageBytes);
            }


            
            var product = new Product(name, price, categoryId, stock, description, path);
            
            await _productRepo.AddAsync(product);
            
            await _productRepo.SaveChangesAsync();
            
            return product;
        }


        public async Task DeleteAsync(Guid id)
        {
            
            var p = await _productRepo.GetAsync(id) ?? throw new InvalidOperationException("Product not found");
            
            if (!string.IsNullOrEmpty(p.ImagePath)) 
                await _imageStorage.DeleteAsync(p.ImagePath);
            
            p.Deactivate();
            
            await _productRepo.UpdateAsync(p);
            
            await _productRepo.SaveChangesAsync();
        }


        public async Task<Product?> GetAsync(Guid id) => await _productRepo.GetAsync(id);
        public async Task<List<Product>> ListActiveAsync() => await _productRepo.ListActiveAsync();

        public async Task<List<Product>> GetAllAsync() => await _productRepo.GetAllAsync();

        public async Task UpdateAsync(Product product, string? imageFileName = null, byte[]? imageBytes = null)
        {
            if (product == null) 
                throw new ArgumentNullException(nameof(product));
            
            if (imageFileName != null 
                && imageBytes != null 
                && imageBytes.Length > 0)
            {
                if (!string.IsNullOrEmpty(product.ImagePath)) 
                    await _imageStorage.DeleteAsync(product.ImagePath);
                
                var path = await _imageStorage.SaveProductImageAsync(imageFileName, imageBytes);

                product.SetImagePath(path);
            }

            await _productRepo.UpdateAsync(product);
            
            await _productRepo.SaveChangesAsync();
        }
    }
}
