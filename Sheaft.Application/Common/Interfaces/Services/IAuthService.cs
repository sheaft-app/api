using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.Application.Common.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result> UpdateUserAsync(IdentityUserInput user, CancellationToken token);
        Task<Result> UpdateUserPictureAsync(IdentityPictureInput user, CancellationToken token);
        Task<Result> RemoveUserAsync(Guid userId, CancellationToken token);
    }
}