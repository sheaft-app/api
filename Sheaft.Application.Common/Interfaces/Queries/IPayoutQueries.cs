using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IPayoutQueries
    {
        IQueryable<PayoutDto> GetPayout(Guid id, RequestUser currentUser);
        IQueryable<PayoutDto> GetPayouts(RequestUser currentUser);
    }
}