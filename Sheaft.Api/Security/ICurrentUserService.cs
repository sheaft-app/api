using Sheaft.Application;
using Sheaft.Domain;
using Sheaft.Domain.Common;

namespace Sheaft.Api.Security
{
    public interface ICurrentUserService
    {
        Result<RequestUser> GetCurrentUserInfo();
    }
}