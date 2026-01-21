using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace restaurant.Model
{
    public class ApplicationUser :IdentityUser<Guid>
{
    
        // الاسم الكامل للمستخدم    
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
    

        // العنوان (اختياري)
        [MaxLength(200)]
        public string? Address { get; set; }

        // صورة البروفايل (اختياري)
        public string? ProfileImageUrl { get; set; }

        // تاريخ إنشاء الحساب
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // حالة الحساب (نشط / محظور / غير مفعل)
        public bool IsActive { get; set; } = true;

        // تاريخ آخر تسجيل دخول
        public DateTime? LastLoginDate { get; set; }

        // لو عايز تربط المستخدم بفرع أو جهة معينة (مثلاً مطعم)
        public int? RestaurantId { get; set; }

        // لو في علاقة مع كيان تاني
        // public virtual Restaurant? Restaurant { get; set; }

        // ممكن تضيف بيانات إضافية حسب المشروع
        public string? Gender { get; set; }  // ذكر / أنثى
        public DateTime? BirthDate { get; set; }
    }
}
