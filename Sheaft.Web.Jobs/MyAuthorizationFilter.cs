using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.States;
using Hangfire.Storage;
using System;
using Microsoft.Extensions.Options;
using Sheaft.Options;

namespace Sheaft.Web.Jobs
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly RoleOptions _roleOptions;

        public MyAuthorizationFilter(IOptionsSnapshot<RoleOptions> roleOptions)
        {
            _roleOptions = roleOptions.Value;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext.User.Identity == null)
                return false;

            return httpContext.User.Identity.IsAuthenticated && (httpContext.User.IsInRole(_roleOptions.Admin.Value) ||
                                                                 httpContext.User.IsInRole(_roleOptions.Support.Value));
        }
    }
}