
using System.ComponentModel.DataAnnotations;

namespace restaurant.Dtos
{
    public class InventoryCreateDto
    {
        [Required]
        public int MenuItemId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}