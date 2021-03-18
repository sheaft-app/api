using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Services.User.Services
{
    public class CurrentUserService : SheaftService, ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<CurrentUserService> logger) : base(logger)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Result<RequestUser> GetCurrentUserInfo()
        {
            if (_httpContextAccessor.HttpContext == null)
                return Result<RequestUser>.Success(new RequestUser());

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return Result<RequestUser>.Success(
                    _httpContextAccessor.HttpContext.User.ToIdentityUser(_httpContextAccessor.HttpContext
                        .TraceIdentifier));

            return Result<RequestUser>.Success(new RequestUser(_httpContextAccessor.HttpContext.TraceIdentifier));
        }
    }
}