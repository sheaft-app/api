﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
                if(NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction != null)
                    NewRelic.Api.Agent.NewRelic.SetTransactionName("SEO", "Robots");

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
