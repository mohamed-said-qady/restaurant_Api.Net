namespace restaurant.Dtos
{
    // لإنشاء عنصر في المنيو
    public class MenuItemCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}