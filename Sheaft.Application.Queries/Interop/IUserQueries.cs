using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface IUserQueries
    {
        Task<string> GetFreshdeskTokenAsync(RequestUser currentUser, CancellationToken token);
        IQueryable<UserDto> GetUserProfile(Guid id, RequestUser currentUser);
        IQueryable<UserDto> GetUser(Guid id, RequestUser currentUser);
        IQueryable<UserDto> GetUsers(RequestUser currentUser);
    }
}