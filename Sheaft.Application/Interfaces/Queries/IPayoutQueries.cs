using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IPayoutQueries
    {
        IQueryable<PayoutDto> GetPayout(Guid id, RequestUser currentUser);
        IQueryable<PayoutDto> GetPayouts(RequestUser currentUser);
    }
}