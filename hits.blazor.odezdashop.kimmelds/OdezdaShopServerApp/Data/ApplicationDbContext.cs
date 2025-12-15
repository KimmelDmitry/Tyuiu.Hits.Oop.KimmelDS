using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OdezdaShopServerApp.Core.Entities;
using OdezdaShopServerApp.Data.Configurations;
using Polly;

namespace OdezdaShopServerApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) => Database.EnsureCreated();


    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.HasDiscriminator<string>("Discriminator")
                .HasValue<ApplicationUser>("ApplicationUser");
        });

        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new OrderItemConfiguration());
    }


    public async Task SeedDatabaseAsync(IServiceProvider serviceProvider, CancellationToken ct = default)
    {
        // Проверяем, есть ли уже какие-то продукты
        if (await Products.AnyAsync())
        {
            Console.WriteLine("База данных уже содержит данные. Инициализация пропущена.");
            return;
        }

        Console.WriteLine("Инициализация базы данных...");

        // Используем Policy для повторных попыток при ошибках подключения
        var retryPolicy = Policy
            .Handle<Exception>() // Обрабатываем любое исключение
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // Экспоненциальная задержка
            );

        await retryPolicy.ExecuteAsync(async () =>
        {
            using var transaction = await Database.BeginTransactionAsync(ct);
            try
            {

                // Генератор для Category 
                var categoryFaker = new Faker<Category>("ru")
                    .RuleFor(c => c.Id, f => Guid.NewGuid()) 
                    .RuleFor(c => c.Name, f => f.Commerce.Department()); 

                // Создаем список начальных категорий
                List<Category> categories = categoryFaker.Generate(6); // Генерируем 6 категорий

                foreach (var category in categories)
                {
                    await Categories.AddAsync(category, ct); 

                }

                await SaveChangesAsync(ct); // Сохраняем категории, чтобы получить их Id

                // Генератор для Product
                var productFaker = new Faker<Product>("ru")
                    .RuleFor(p => p.Id, f => Guid.NewGuid()) 
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                    .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
                    .RuleFor(p => p.IsActive, f => true) 
                    .RuleFor(p => p.Stock, f => f.Random.Number(10, 100)) 
                    .RuleFor(p => p.ImagePath, f => "/images/placeholder_product.jpg") 
                    .RuleFor(p => p.CategoryId, f => f.PickRandom(categories.Select(c => c.Id)));

                // Создаем список начальных продуктов
                var products = productFaker.Generate(20); // Генерируем 20 продуктов
                await Products.AddRangeAsync(products, ct);
                await SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
                Console.WriteLine("База данных успешно инициализирована с тестовыми данными (Bogus).");
            }
            catch (Exception ex) // Ловим любое исключение
            {
                await transaction.RollbackAsync(ct);
                Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}. Транзакция откатилась.");
                throw; // Перебросить исключение для обработки в Program.cs
            }
        });
    }
}



