using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Sheaft.Domain;

namespace Sheaft.Application.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return string.Empty;

            return user.FindFirst(JwtClaimTypes.Name)?.Value;
        }

        public static string GetFirstName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return string.Empty;

            return user.FindFirst(JwtClaimTypes.GivenName)?.Value;
        }

        public static string GetLastName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return string.Empty;

            return user.FindFirst(JwtClaimTypes.FamilyName)?.Value;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return string.Empty;

            return user.FindFirst(JwtClaimTypes.Email)?.Value;
        }

        public static Guid? TryGetUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            var userClaim = user.FindFirst(JwtClaimTypes.Subject)?.Value;
            if (string.IsNullOrWhiteSpace(userClaim))
                return null;

            var id = Guid.Parse(userClaim);
            if (id == Guid.Empty)
                return null;

            return id;
        }

        public static List<string> GetRoles(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return new List<string>();

            var userClaims = user.FindAll(JwtClaimTypes.Role);
            return userClaims.Select(uc => uc.Value)?.Distinct().ToList();
        }

        public static RequestUser ToIdentityUser(this ClaimsPrincipal user, string requestId, Impersonification impersonification = null)
        {
            var email = user.Claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value;
            var name = user.Claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value;

            return new RequestUser(user.TryGetUserId(), user.GetName(), user.GetEmail(), user.GetRoles(), requestId, impersonification);
        }
    }
}
