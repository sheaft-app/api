using System.Security.Claims;
using Sheaft.Domain;
using Sheaft.Infrastructure;

namespace Sheaft.Web.Api
{
    public static class ClaimsPrincipalExtensions
    {
        private static ProfileKind? TryGetUserKind(this ClaimsPrincipal user)
        {
            return user.Identity is {IsAuthenticated: true} 
                ? user.FindFirstValue(CustomClaims.ProfileKind) != null 
                    ? Enum.Parse<ProfileKind>(user.FindFirstValue(CustomClaims.ProfileKind)) : null
                : null;
        }
        
        private static AccountId TryGetAccountId(this ClaimsPrincipal user)
        {
            return user.Identity is {IsAuthenticated: true} 
                ? new AccountId(user.FindFirstValue(CustomClaims.AccountIdentifier))
                : null;
        }
        
        private static string TryGetProfileId(this ClaimsPrincipal user)
        {
            return user.Identity is {IsAuthenticated: true} 
                ? user.FindFirstValue(CustomClaims.ProfileIdentifier)
                : null;
        }

        public static RequestUser ToIdentityUser(this ClaimsPrincipal user)
        {
            return new RequestUser(user.Identity?.IsAuthenticated ?? false, user.TryGetUserKind(), user.TryGetAccountId(), user.TryGetProfileId());
        }
    }
}
