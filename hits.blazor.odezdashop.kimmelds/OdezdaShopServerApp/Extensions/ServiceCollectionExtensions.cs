using OdezdaShopServerApp.Core.Interfaces.Repositories;
using OdezdaShopServerApp.Core.Interfaces.Services;
using OdezdaShopServerApp.Core.Repositories;
using OdezdaShopServerApp.Core.Services;
using OdezdaShopServerApp.VerticalSlices;

namespace OdezdaShopServerApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStoreInfrastructure(this IServiceCollection services, IWebHostEnvironment env)
    {

        services.AddScoped<ICartService, InMemoryCartService>();

        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<ICategoryRepository, EfCategoryRepository>();
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<IImageStorageService>(_ => new FileSystemImageStorageService(env.WebRootPath));
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();


        services.AddScoped<ICategoryService, CategoryService>();



        return services;
    }


}
