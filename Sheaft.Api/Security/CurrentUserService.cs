using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
                return Result<RequestUser>.Success(new RequestUser());

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return Result<RequestUser>.Success(
                    _httpContextAccessor.HttpContext.User.ToIdentityUser(_httpContextAccessor.HttpContext
                        .TraceIdentifier));

            return Result<RequestUser>.Success(new RequestUser(_httpContextAccessor.HttpContext.TraceIdentifier));
        }
    }
}