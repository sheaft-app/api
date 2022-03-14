using Sheaft.Domain;

namespace Sheaft.Api
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Result<RequestUser> GetCurrentUserInfo()
        {
            if (_httpContextAccessor.HttpContext?.User?.Identity == null)
                return Result.Success(new RequestUser(false));

            return Result.Success(_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated 
                ? _httpContextAccessor.HttpContext.User.ToIdentityUser() 
                : new RequestUser(false));
        }
    }
}