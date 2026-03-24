using Microsoft.AspNetCore.Authorization;

namespace restaurant.Authorization
{
    // ده مجرد "اختصار" عشان بدل ما نكتب [Authorize(Policy = "Order.Create")]
    // نكتب [Permission("Order.Create")] علطول
    public class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute(string permission) : base(permission)
        {
        }
    }
}