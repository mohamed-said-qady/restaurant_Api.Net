using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using restaurant.Helper; // اتأكد إن السيمي كولون موجودة هنا

namespace restaurant.Services.Interfaces
{
    public interface IRoleService
    {
        // جلب دور معين بمعلومية المعرف
        Task<ServiceResult<IdentityRole<Guid>>> GetByIdAsync(Guid id);

        // جلب كل الأدوار الموجودة في النظام
        Task<ServiceResult<IEnumerable<IdentityRole<Guid>>>> GetAllAsync();

        // إنشاء دور جديد
        Task<ServiceResult<IdentityRole<Guid>>> CreateAsync(string roleName);

        // تحديث اسم دور موجود
        // يفضل تمرير المعرف والاسم الجديد لضمان الدقة
        Task<ServiceResult<IdentityRole<Guid>>> UpdateAsync(Guid id, string newName);

        // حذف دور
        Task<ServiceResult<bool>> DeleteAsync(Guid id);
    }
}