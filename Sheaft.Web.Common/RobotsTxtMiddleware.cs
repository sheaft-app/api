using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Sheaft.Web.Common
{
    public class RobotsTxtMiddleware
    {
        const string Default = "User-agent: *  \nDisallow: /";
        private readonly RequestDelegate next;

        public RobotsTxtMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.StartsWith("/robots"))
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(Default);
            }
            else
            {
                await next(context);
            }
        }
    }
}
