using Sheaft.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Core.Extensions
{
    public static class IUserExtensions
    {
        public static bool IsInRole(this RequestUser user, string roleName)
        {
            if (user == null || user.Roles == null || !user.Roles.Any())
                return false;

            return user.Roles.Contains(roleName);
        }
        
        public static bool IsInRoles(this RequestUser user, IEnumerable<string> roleNames)
        {
            return roleNames.All(user.IsInRole);
        }

        public static bool IsInAnyRoles(this RequestUser user, IEnumerable<string> roleNames)
        {
            return roleNames.Any(user.IsInRole);
        }
    }

    public static class CoreProductExtensions
    {
        public static string GetImageUrl(Guid companyId, Guid productId, string filename, string size)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return null;

            return $"companies/{companyId}/products/{productId}/{filename}_{size}.jpg";
        }
        public static string GetImageUrl(Guid companyId, Guid productId, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return null;

            return $"companies/{companyId}/products/{productId}/{filename}";
        }
        public static string GetImageUrl(string image, string size)
        {
            if (string.IsNullOrWhiteSpace(image))
                return null;

            return $"{image}_{size}.jpg";
        }
    }
}