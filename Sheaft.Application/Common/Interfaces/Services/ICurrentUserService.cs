using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Result<RequestUser> GetCurrentUserInfo();
    }
}