using System.ComponentModel.DataAnnotations;

namespace restaurant.Dtos
{
    public class InventoryUpdateDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
