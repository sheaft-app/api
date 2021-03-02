using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IDeliveryClosingQueries
    {
        IQueryable<ClosingDto> GetClosing(Guid id, RequestUser currentUser);
        IQueryable<ClosingDto> GetClosings(RequestUser currentUser);
    }
}