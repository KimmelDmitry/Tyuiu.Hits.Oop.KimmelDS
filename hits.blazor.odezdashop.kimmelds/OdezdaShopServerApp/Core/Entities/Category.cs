namespace OdezdaShopServerApp.Core.Entities;

public sealed class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }


    // Navigation
    public ICollection<Product> Products { get; set; } = new List<Product>();


    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        Name = name;
    }


    public Category() { } // EF


    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException(nameof(newName));
        Name = newName;
    }
}
