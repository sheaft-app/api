using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface ILegalQueries
    {
        IQueryable<ConsumerLegalDto> GetMyConsumerLegals(RequestUser currentUser);
        IQueryable<BusinessLegalDto> GetMyBusinessLegals(RequestUser currentUser);
        IQueryable<ConsumerLegalDto> GetConsumerLegals(Guid id, RequestUser currentUser);
        IQueryable<BusinessLegalDto> GetBusinessLegals(Guid id, RequestUser currentUser);
    }
}