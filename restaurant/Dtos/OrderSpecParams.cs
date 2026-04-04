namespace restaurant.Dtos
{
    public class OrderSpecParams
    {
        // الفلاتر
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? CustomerId { get; set; }
        public string? Status { get; set; }

        // الـ Pagination (مع الحماية اللي عملناها)
        private int _pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 50) ? 50 : value;
        }
    }
}