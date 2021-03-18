using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IAgreementQueries
    {
        IQueryable<AgreementDto> GetAgreement(Guid id, RequestUser currentUser);
        IQueryable<AgreementDto> GetAgreements(RequestUser currentUser);
        IQueryable<AgreementDto> GetStoreAgreements(Guid input, RequestUser currentUser);
        IQueryable<AgreementDto> GetProducerAgreements(Guid producerId, RequestUser currentUser);
    }
}