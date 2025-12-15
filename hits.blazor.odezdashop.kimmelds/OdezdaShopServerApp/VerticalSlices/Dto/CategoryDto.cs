namespace OdezdaShopServerApp.VerticalSlices.Dto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProductCount { get; set; } // удобная метрика
    }
}
