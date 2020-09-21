using Sheaft.Core;
using Sheaft.Application.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface IUserQueries
    {
        Task<string> GetFreshdeskTokenAsync(RequestUser currentUser, CancellationToken token);
        IQueryable<UserDto> GetUser(RequestUser currentUser);
        IQueryable<UserProfileDto> GetUserProfile(RequestUser currentUser);
    }
}