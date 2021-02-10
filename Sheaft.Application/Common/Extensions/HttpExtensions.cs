using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Sheaft.Core.Extensions
{

    public static class HttpExtensions
    {
        public static Guid? ImpersonificationId(this HttpRequest request)
        {
            var cookie = request.Cookies.FirstOrDefault(c => c.Key == "Sheaft.Impersonating.Id");
            if (cookie.Value != null)
            {
                return Guid.Parse(cookie.Value);
            }

            return null;
        }
        public static string ImpersonificationName(this HttpRequest request)
        {
            var cookie = request.Cookies.FirstOrDefault(c => c.Key == "Sheaft.Impersonating.Name");
            if (cookie.Value != null)
            {
                return cookie.Value;
            }

            return null;
        }
    }
}
