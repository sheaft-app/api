using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface ICurrentUserService
    {
        Result<RequestUser> GetCurrentUserInfo();
    }
}