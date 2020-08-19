using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface ICompanyQueries
    {
        IQueryable<ProducerDto> GetProducer(Guid id, IRequestUser currentUser);
        IQueryable<StoreDto> GetStore(Guid id, IRequestUser currentUser);
        IQueryable<CompanyDto> GetCompany(Guid id, IRequestUser currentUser);
        IQueryable<CompanyDto> GetCompanies(IRequestUser currentUser);
        Task<SirenCompanyDto> RetrieveSiretCompanyInfosAsync(string siret, IRequestUser currentUser, CancellationToken token);
        Task<ProducersSearchDto> SearchProducersAsync(Guid companyId, SearchTermsInput terms, IRequestUser currentUser, CancellationToken token);
        Task<StoresSearchDto> SearchStoresAsync(Guid companyId, SearchTermsInput terms, IRequestUser currentUser, CancellationToken token);
    }
}