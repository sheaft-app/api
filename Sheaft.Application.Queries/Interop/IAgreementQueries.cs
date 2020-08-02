using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IAgreementQueries
    {
        IQueryable<AgreementDto> GetAgreement(Guid id, IRequestUser currentUser);
        IQueryable<AgreementDto> GetAgreements(IRequestUser currentUser);
        IQueryable<AgreementDto> GetStoreAgreements(Guid input, IRequestUser currentUser);
        IQueryable<AgreementDto> GetProducerAgreements(Guid producerId, IRequestUser currentUser);
    }
}