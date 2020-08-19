using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Sheaft.Core.Security;

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
