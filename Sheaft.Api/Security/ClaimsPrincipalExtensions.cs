using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Sheaft.Domain.Common;

namespace Sheaft.Api.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetName(this ClaimsPrincipal user)
        {
            if (user.Identity is {IsAuthenticated: false})
                return string.Empty;

            return user.FindFirst("name")?.Value;
        }

        public static string GetFirstName(this ClaimsPrincipal user)
        {
            if (user.Identity is {IsAuthenticated: false})
                return string.Empty;

            return user.FindFirst("givenName")?.Value;
        }

        public static string GetLastName(this ClaimsPrincipal user)
        {
            if (user.Identity is {IsAuthenticated: false})
                return string.Empty;

            return user.FindFirst("familyName")?.Value;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            if (user.Identity is {IsAuthenticated: false})
                return string.Empty;

            return user.FindFirst("email")?.Value;
        }

        public static Guid? TryGetUserId(this ClaimsPrincipal user)
        {
            if (user.Identity is {IsAuthenticated: false})
                return null;

            var userClaim = user.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userClaim))
                return null;

            var id = Guid.Parse(userClaim);
            if (id == Guid.Empty)
                return null;

            return id;
        }

        public static Guid? TryGetCompanyId(this ClaimsPrincipal user)
        {
            if (user.Identity is {IsAuthenticated: false})
                return null;

            var companyClaim = user.FindFirst("company")?.Value;
            if (string.IsNullOrWhiteSpace(companyClaim))
                return null;

            var id = Guid.Parse(companyClaim);
            if (id == Guid.Empty)
                return null;

            return id;
        }

        public static List<string> GetRoles(this ClaimsPrincipal user)
        {
            if (user.Identity is {IsAuthenticated: false})
                return new List<string>();

            var userClaims = user.FindAll("role");
            return userClaims.Select(uc => uc.Value)?.Distinct().ToList();
        }

        public static RequestUser ToIdentityUser(this ClaimsPrincipal user, string requestId, Impersonification impersonification = null)
        {
            return new RequestUser(user.TryGetUserId(), user.GetName(), user.GetEmail(), user.GetRoles(), user.TryGetCompanyId(), requestId, impersonification);
        }
    }
}
