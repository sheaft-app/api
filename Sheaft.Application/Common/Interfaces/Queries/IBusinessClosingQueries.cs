using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.BusinessClosing.Queries
{
    public interface IBusinessClosingQueries
    {
        IQueryable<ClosingDto> GetClosing(Guid id, RequestUser currentUser);
        IQueryable<ClosingDto> GetClosings(RequestUser currentUser);
    }
}