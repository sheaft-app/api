using Sheaft.Domain;

namespace Sheaft.Api
{
    public interface ICurrentUserService
    {
        Result<RequestUser> GetCurrentUserInfo();
    }
}