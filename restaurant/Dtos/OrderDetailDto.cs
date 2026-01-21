namespace restaurant.Dtos
{
    // تفاصيل عنصر داخل الطلب عند عرض الطلب
    public class OrderDetailDto
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}