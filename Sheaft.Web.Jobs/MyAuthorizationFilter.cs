using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sheaft.Core.Options;

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
    
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter {
        private readonly string _policyName;

        public HangfireAuthorizationFilter(string policyName) {
            _policyName = policyName;
        }

        public bool Authorize([NotNull] DashboardContext context) {
            var httpContext = context.GetHttpContext();
            var authService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            return authService.AuthorizeAsync(httpContext.User, _policyName).ConfigureAwait(false).GetAwaiter().GetResult().Succeeded;
        }
    }
}