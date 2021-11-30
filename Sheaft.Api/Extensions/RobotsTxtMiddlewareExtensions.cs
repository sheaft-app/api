using Microsoft.AspNetCore.Builder;
using Sheaft.Api.Middlewares;

namespace Sheaft.Api.Extensions
{
    public static class RobotsTxtMiddlewareExtensions
    {
        public static IApplicationBuilder UseRobotsTxt(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RobotsTxtMiddleware>();
        }
    }
}
