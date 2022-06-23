using Sheaft.Domain;

namespace Sheaft.Infrastructure
{
    public interface ICurrentUserService
    {
        Result<RequestUser> GetCurrentUserInfo();
    }
}