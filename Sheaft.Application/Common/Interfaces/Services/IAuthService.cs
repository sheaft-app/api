using Sheaft.Application.Models;
using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IAuthService
    {
        Task<Result<bool>> UpdateUserAsync(IdentityUserInput user, CancellationToken token);
        Task<Result<bool>> UpdateUserPictureAsync(IdentityPictureInput user, CancellationToken token);
        Task<Result<bool>> RemoveUserAsync(Guid userId, CancellationToken token);
    }
}