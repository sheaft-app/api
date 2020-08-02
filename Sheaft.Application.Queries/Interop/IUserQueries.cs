using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface IUserQueries
    {
        Task<string> GetFreshdeskTokenAsync(IRequestUser currentUser, CancellationToken token);
        IQueryable<UserDto> GetUserProfile(Guid id, IRequestUser currentUser);
        IQueryable<UserDto> GetUser(Guid id, IRequestUser currentUser);
        IQueryable<UserDto> GetUsers(IRequestUser currentUser);
    }
}