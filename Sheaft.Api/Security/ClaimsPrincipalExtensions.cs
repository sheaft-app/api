using System.Security.Claims;
using Sheaft.Application;
using Sheaft.Domain;

namespace Sheaft.Api.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static long? TryGetUserId(this ClaimsPrincipal user)
        {
            if (user.Identity is {IsAuthenticated: false})
                return null;

            var userClaim = user.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userClaim))
                return null;

            if (long.TryParse(userClaim, out var userId))
                return userId;

            return null;
        }

        public static RequestUser ToIdentityUser(this ClaimsPrincipal user)
        {
            return new RequestUser(user.TryGetUserId());
        }
    }
}
