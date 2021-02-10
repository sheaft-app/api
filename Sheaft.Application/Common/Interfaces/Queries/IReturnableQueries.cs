using Sheaft.Application.Models;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IReturnableQueries
    {
        IQueryable<ReturnableDto> GetReturnable(Guid id, RequestUser currentUser);
        IQueryable<ReturnableDto> GetReturnables(RequestUser currentUser);
    }
}