using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IReturnableQueries
    {
        IQueryable<ReturnableDto> GetReturnable(Guid id, RequestUser currentUser);
        IQueryable<ReturnableDto> GetReturnables(RequestUser currentUser);
    }
}