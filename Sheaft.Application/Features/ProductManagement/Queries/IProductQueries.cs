using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public interface IProductQueries
{
    Task<Result<ProductDto>> Get(ProductId identifier, CancellationToken token);
    Task<Result<PagedResult<ProductDto>>> List(SupplierId supplierId, PageInfo pageInfo, CancellationToken token);
}