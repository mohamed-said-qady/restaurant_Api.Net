using System.ComponentModel.DataAnnotations;

namespace restaurant.Dtos
{
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "لازم تختار صنف واحد على الأقل")]
        public List<OrderItemCreateDto> OrderDetails { get; set; } = new();

        [MaxLength(500, ErrorMessage = "الملاحظات كبيرة جداً، اختصر يا هندسة")]
        public string? CustomerNotes { get; set; }

        [MaxLength(250, ErrorMessage = "العنوان طويل زيادة عن اللزوم")]
        public string? DeliveryAddress { get; set; }
    }
}