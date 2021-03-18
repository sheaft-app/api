using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IUserQueries
    {
        Task<string> GetFreshdeskTokenAsync(RequestUser currentUser, CancellationToken token);
        IQueryable<UserDto> GetUser(Guid id, RequestUser currentUser);
        Task<SirenBusinessDto> RetrieveSiretInfosAsync(string input, RequestUser currentUser, CancellationToken token);
    }
}