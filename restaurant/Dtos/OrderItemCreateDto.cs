namespace restaurant.Dtos
{
    // يمثل عنصر واحد داخل الطلب
    public class OrderItemCreateDto
    {
        public int MenuItemId { get; set; }   // معرف العنصر
        public int Quantity { get; set; }     // الكمية المطلوبة
    }
}