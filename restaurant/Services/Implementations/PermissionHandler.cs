using Microsoft.AspNetCore.Authorization;
using restaurant.Authorization;
using System.Linq;
using System.Threading.Tasks;




namespace restaurant.Services.Implementations
{
	public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
	{
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context
			, PermissionRequirement requirement)
		{
			var permissions = context.User.Claims.
				Where(c => c.Type == "Permission")
				.Select(c => c.Value)
				.ToArray();
			if (permissions.Contains(requirement.Permission))
			{
				context.Succeed(requirement);
			}
			return Task.CompletedTask;

		}
	}
}
