using System.ComponentModel.DataAnnotations;

namespace OdezdaShopServerApp.VerticalSlices.Dto
{
    public class CategoryCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;
    }
}
