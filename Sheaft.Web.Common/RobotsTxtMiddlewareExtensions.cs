using Microsoft.AspNetCore.Builder;

namespace Sheaft.Web.Common
{
    public static class RobotsTxtMiddlewareExtensions
    {
        public static IApplicationBuilder UseRobotsTxt(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RobotsTxtMiddleware>();
        }
    }
}
