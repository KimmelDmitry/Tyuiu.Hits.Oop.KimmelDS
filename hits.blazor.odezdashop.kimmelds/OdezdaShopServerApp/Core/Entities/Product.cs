namespace OdezdaShopServerApp.Core.Entities;

public sealed class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; } = true;


    // .../images/products/xxx.jpg)
    public string? ImagePath { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public Product(string name, decimal price, Guid categoryId, int stock = 0, string? description = null, string? imagePath = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
        Name = name;
        Price = price;
        CategoryId = categoryId;
        Stock = stock;
        Description = description;
        ImagePath = imagePath;
    }


    public Product() { } // EF


    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0) throw new ArgumentOutOfRangeException(nameof(newPrice));
        Price = newPrice;
    }


    public void AdjustStock(int delta)
    {
        var newStock = Stock + delta;
        if (newStock < 0) throw new InvalidOperationException("Insufficient stock");
        Stock = newStock;
    }


    public void Deactivate() => IsActive = false;


    public void SetImagePath(string path) => ImagePath = path;
}
