namespace restaurant.Dtos
{
    public class MenuItemSpecParams
    {

        public string Name { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int _PageSize { get; set; } = 10;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > 50) ? 50 : value;
        }


    }
}
