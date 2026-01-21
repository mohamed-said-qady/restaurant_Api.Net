namespace restaurant.Dtos
{
    // لتحديث عنصر في المنيو
    public class MenuItemUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}