using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IAgreementQueries
    {
        IQueryable<AgreementDto> GetAgreement(Guid id, RequestUser currentUser);
        IQueryable<AgreementDto> GetAgreements(RequestUser currentUser);
        IQueryable<AgreementDto> GetStoreAgreements(Guid input, RequestUser currentUser);
        IQueryable<AgreementDto> GetProducerAgreements(Guid producerId, RequestUser currentUser);
    }
}