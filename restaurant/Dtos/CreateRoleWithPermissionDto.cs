using System.ComponentModel.DataAnnotations;

namespace restaurant.Dtos
{
    public class CreateRoleWithPermissionDto
    {
        [Required(ErrorMessage = "هذا الدور مطلوب")]
        //public string RoleId { get; set; }
        public string RoleName { get; set; }

        public List<int> PermissionIds { get; set; } = new List<int>();
    }
}
