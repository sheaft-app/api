using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface IAuthService
    {
        Task<Result> UpdateUserAsync(IdentityUserDto user, CancellationToken token);
        Task<Result> UpdateUserPictureAsync(IdentityPictureDto user, CancellationToken token);
        Task<Result> RemoveUserAsync(Guid userId, CancellationToken token);
    }
}