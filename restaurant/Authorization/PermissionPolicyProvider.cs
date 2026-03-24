
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace restaurant.Authorization
{

    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // بنشغل الـ Default عشان الـ [Authorize] العادية تفضل شغالة
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // 1. شوف لو السيستم فيه Policy ثابتة بالاسم ده
            var policy = await FallbackPolicyProvider.GetPolicyAsync(policyName);
            if (policy != null) return policy;

            // 2. لو مفيش، اصنع واحدة فوراً وضيف لها الـ Requirement بتاعنا
            // هنا الـ policyName هو نفسه "Order.View" أو أي كلمة تانية هتكتبها
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}