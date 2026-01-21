namespace restaurant.Dtos
{
    // يمثل الطلب كامل مع كل العناصر المطلوبة
    public class OrderCreateDto
    {
        public List<OrderItemCreateDto> OrderDetails { get; set; } = new();
    }
}