using Sheaft.Domain;

namespace Sheaft.Web.Api
{
    public interface ICurrentUserService
    {
        Result<RequestUser> GetCurrentUserInfo();
    }
}