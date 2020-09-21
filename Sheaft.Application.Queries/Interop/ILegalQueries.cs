using Sheaft.Core;
using Sheaft.Application.Models;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ILegalQueries
    {
        IQueryable<ConsumerLegalDto> GetMyConsumerLegals(RequestUser currentUser);
        IQueryable<BusinessLegalDto> GetMyBusinessLegals(RequestUser currentUser);
        IQueryable<ConsumerLegalDto> GetConsumerLegals(Guid id, RequestUser currentUser);
        IQueryable<BusinessLegalDto> GetBusinessLegals(Guid id, RequestUser currentUser);
    }
}