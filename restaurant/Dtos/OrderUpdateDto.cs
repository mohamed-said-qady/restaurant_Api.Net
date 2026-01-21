namespace restaurant.Dtos
{
    // لتحديث حالة الطلب (Status)
    public class OrderUpdateDto
    {
        public string Status { get; set; } = "Pending";
    }
}