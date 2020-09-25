using Sheaft.Core;
using Sheaft.Application.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Sheaft.Application.Queries
{
    public interface IUserQueries
    {
        Task<string> GetFreshdeskTokenAsync(RequestUser currentUser, CancellationToken token);
        IQueryable<UserDto> GetUser(Guid id, RequestUser currentUser);
        IQueryable<UserProfileDto> GetUserProfile(Guid id, RequestUser currentUser);
    }
}