namespace restaurant.Dtos
{
    // لإضافة كمية جديدة للمخزون أو إعادة التخزين
    public class RestockInventoryDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}