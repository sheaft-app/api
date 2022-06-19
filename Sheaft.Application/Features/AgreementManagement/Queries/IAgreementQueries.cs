using Sheaft.Application.CustomerManagement;
using Sheaft.Application.SupplierManagement;
using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public interface IAgreementQueries
{
    Task<Result<AgreementDetailsDto>> Get(AgreementId identifier, AccountId accountId, CancellationToken token);
    Task<Result<PagedResult<AgreementDto>>> ListActiveAgreements(AccountId accountId, PageInfo pageInfo, CancellationToken token, string? requestSearch = null);
    Task<Result<PagedResult<AgreementDto>>> ListSentAgreements(AccountId accountId, PageInfo pageInfo, CancellationToken token, string? requestSearch = null);
    Task<Result<PagedResult<AgreementDto>>> ListReceivedAgreements(AccountId accountId, PageInfo pageInfo, CancellationToken token, string? requestSearch = null);
    Task<Result<AvailableCustomerDto>> GetCustomer(CustomerId identifier, CancellationToken token);
    Task<Result<AvailableSupplierDto>> GetSupplier(SupplierId identifier, CancellationToken token);
    Task<Result<PagedResult<AvailableCustomerDto>>> ListAvailableCustomersForSupplier(SupplierId supplierId, PageInfo pageInfo, CancellationToken token);
    Task<Result<PagedResult<AvailableSupplierDto>>> ListAvailableSuppliersForCustomer(CustomerId customerId, PageInfo pageInfo, CancellationToken token);
}