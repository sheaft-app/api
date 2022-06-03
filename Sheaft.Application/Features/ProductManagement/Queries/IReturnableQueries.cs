using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public interface IReturnableQueries
{
    Task<Result<ReturnableDto>> Get(ReturnableId identifier, CancellationToken token);
    Task<Result<PagedResult<ReturnableDto>>> List(SupplierId supplierId, PageInfo pageInfo, CancellationToken token);
}