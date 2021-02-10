using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Core.Extensions
{
    public static class RequestUserExtensions
    {
        public static bool IsInRole(this RequestUser user, string roleName)
        {
            if (user == null || user.Roles?.Any() != true)
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
}