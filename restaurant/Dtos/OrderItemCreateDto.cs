namespace restaurant.Dtos
{
               //       يمثل عنصر واحد داخل الطل
    public class OrderItemCreateDto
    {
        public int MenuItemId { get; set; }   // معرف العنصر
        public int Quantity { get; set; }     // الكمية المطلوبة
    }
}