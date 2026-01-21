namespace restaurant.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }   // FK

        public int MenuItemId { get; set; } // FK

        public MenuItem? MenuItem { get; set; } // Navigation property (optional)

        public int Quantity { get; set; }
        public decimal Price { get; set; } // ✅ سعر وقت الطلب
    }
}
