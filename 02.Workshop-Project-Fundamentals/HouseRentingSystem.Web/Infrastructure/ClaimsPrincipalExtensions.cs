using System.Security.Claims;
using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;
#nullable disable

namespace HouseRentingSystem.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
	{
		public static string Id(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.NameIdentifier).Value;
		}

		public static bool IsAdmin(this ClaimsPrincipal user)
		{
			return user.IsInRole(AdminRoleName);
		}
	}
}
