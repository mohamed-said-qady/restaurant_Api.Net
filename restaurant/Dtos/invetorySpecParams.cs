using restaurant.Model;

namespace restaurant.Dtos
{
    public class invetorySpecParams
    {
        public int MenuItemId { get; set; }
        public MenuItemCreateDto? MenuItem { get; set; } = new MenuItemCreateDto();
        public int _PageSize { get; set; } = 10; 
        public int PageNumber { get; set; } = 1;    
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value>50)? 50 :value;
        } 
    }
}
