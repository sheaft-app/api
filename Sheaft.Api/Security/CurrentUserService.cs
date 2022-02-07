using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sheaft.Application;
using Sheaft.Domain;
using Sheaft.Domain.Common;

namespace Sheaft.Api.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<CurrentUserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Result<RequestUser> GetCurrentUserInfo()
        {
            if (_httpContextAccessor.HttpContext?.User?.Identity == null)
                return Result<RequestUser>.Success(new RequestUser(null));

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return Result<RequestUser>.Success(
                    _httpContextAccessor.HttpContext.User.ToIdentityUser());

            return Result<RequestUser>.Success(new RequestUser(null));
        }
    }
}