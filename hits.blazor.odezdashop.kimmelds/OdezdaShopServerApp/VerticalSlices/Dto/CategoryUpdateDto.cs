using System.ComponentModel.DataAnnotations;

namespace OdezdaShopServerApp.VerticalSlices.Dto
{
    public class CategoryUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;
    }
}
