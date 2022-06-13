using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public interface IAgreementQueries
{
    Task<Result<AgreementDetailsDto>> Get(AgreementId identifier, CancellationToken token);
    Task<Result<PagedResult<AgreementDto>>> List(AccountId accountId, PageInfo pageInfo, CancellationToken token);
    Task<Result<AvailableCustomerDto>> GetCustomer(CustomerId identifier, CancellationToken token);
    Task<Result<AvailableSupplierDto>> GetSupplier(SupplierId identifier, CancellationToken token);
    Task<Result<PagedResult<AvailableCustomerDto>>> ListAvailableCustomersForSupplier(SupplierId supplierId, PageInfo pageInfo, CancellationToken token);
    Task<Result<PagedResult<AvailableSupplierDto>>> ListAvailableSuppliersForCustomer(CustomerId customerId, PageInfo pageInfo, CancellationToken token);
}