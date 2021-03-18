using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IReturnableQueries
    {
        IQueryable<ReturnableDto> GetReturnable(Guid id, RequestUser currentUser);
        IQueryable<ReturnableDto> GetReturnables(RequestUser currentUser);
    }
}