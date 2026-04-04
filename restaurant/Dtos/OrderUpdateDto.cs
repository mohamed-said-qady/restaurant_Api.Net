using restaurant.Data.Enums;

namespace restaurant.Dtos
{
    // لتحديث حالة الطلب (Status)
    public class OrderUpdateDto
    {
        // مسموح للأدمن يغير الحالة
        public OrderStatus? Status { get; set; }

        // مسموح للعميل يغير ملاحظاته قبل التحضير
        public string? CustomerNotes { get; set; }

        // مسموح بتغيير العنوان لو الأوردر لسه مخرجش
        public string? DeliveryAddress { get; set; }
    }
}