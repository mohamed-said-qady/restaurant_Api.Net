using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace restaurant.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 1. هات البرمشنز من التوكن (استخدم ToList عشان نقدر نشوفها في الـ Debugger)
            var permissions = context.User.Claims
                .Where(x => x.Type.Equals("permission", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value)
                .ToList();

            // 2. سطر للـ Debugging (مهم جداً): افتح الـ Output Window وشوف القيم الحقيقية
            foreach (var p in permissions)
            {
                Console.WriteLine($"Token Claim: '{p}' | Requirement: '{requirement.Permission}'");
            }

            // 3. المقارنة المرنة (تجاهل حالة الأحرف في الطرفين)
            if (permissions.Any(p => p.Equals(requirement.Permission, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else
            {
                // لو فشل، اطبع رسالة في الـ Console عشان نعرف إنه دخل هنا وما لقاش تطابق
                Console.WriteLine($"Forbidden: No match found for {requirement.Permission}");
            }

            return Task.CompletedTask;
        }
    }
}