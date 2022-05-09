using System.Security.Claims;
using Sheaft.Domain;

namespace Sheaft.Web.Api
{
    public static class ClaimsPrincipalExtensions
    {
        private static string? TryGetUserId(this ClaimsPrincipal user)
        {
            return user.Identity is {IsAuthenticated: true} 
                ? user.FindFirst("sub")?.Value
                : null;
        }

        public static RequestUser ToIdentityUser(this ClaimsPrincipal user)
        {
            return new RequestUser(user.Identity?.IsAuthenticated ?? false, user.TryGetUserId());
        }
    }
}
