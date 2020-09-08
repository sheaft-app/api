using Sheaft.Core;
using Sheaft.Models.Dto;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface IUserQueries
    {
        Task<string> GetFreshdeskTokenAsync(RequestUser currentUser, CancellationToken token);
        IQueryable<UserDto> GetUser(RequestUser currentUser);
    }
}