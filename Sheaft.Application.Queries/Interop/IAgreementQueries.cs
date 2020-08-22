using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IAgreementQueries
    {
        IQueryable<AgreementDto> GetAgreement(Guid id, RequestUser currentUser);
        IQueryable<AgreementDto> GetAgreements(RequestUser currentUser);
        IQueryable<AgreementDto> GetStoreAgreements(Guid input, RequestUser currentUser);
        IQueryable<AgreementDto> GetProducerAgreements(Guid producerId, RequestUser currentUser);
    }
}