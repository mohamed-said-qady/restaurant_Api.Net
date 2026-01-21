
using System;
using Microsoft.AspNetCore.Identity;

namespace restaurant.Model
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public IdentityRole<Guid> Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
