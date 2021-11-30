using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Sheaft.Api.Authorize
{
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