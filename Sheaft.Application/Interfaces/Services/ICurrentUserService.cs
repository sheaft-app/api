using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Result<RequestUser> GetCurrentUserInfo();
    }
}