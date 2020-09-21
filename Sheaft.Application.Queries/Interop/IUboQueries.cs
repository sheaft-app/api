using Sheaft.Core;
using Sheaft.Application.Models;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IUboQueries
    {
        IQueryable<UboDto> GetUbo(Guid id, RequestUser currentUser);
        IQueryable<UboDto> GetUbos(RequestUser currentUser);
    }
}