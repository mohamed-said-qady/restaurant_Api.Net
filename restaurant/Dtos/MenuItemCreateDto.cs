using System.ComponentModel.DataAnnotations;
namespace restaurant.Dtos
{
    // لإنشاء عنصر في المنيو
    public class MenuItemCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}