using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface ILegalQueries
    {
        IQueryable<ConsumerLegalDto> GetMyConsumerLegals(RequestUser currentUser);
        IQueryable<BusinessLegalDto> GetMyBusinessLegals(RequestUser currentUser);
        IQueryable<ConsumerLegalDto> GetConsumerLegals(Guid id, RequestUser currentUser);
        IQueryable<BusinessLegalDto> GetBusinessLegals(Guid id, RequestUser currentUser);
    }
}