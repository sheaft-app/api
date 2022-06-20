using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public record ListOrderableProductsQuery(AccountId CustomerAccountId, SupplierId SupplierId, PageInfo PageInfo) : IQuery<Result<PagedResult<OrderableProductDto>>>;

internal class ListOrderableProductsHandler : IQueryHandler<ListOrderableProductsQuery, Result<PagedResult<OrderableProductDto>>>
{
    private readonly IProductQueries _productQueries;

    public ListOrderableProductsHandler(IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }
    
    public async Task<Result<PagedResult<OrderableProductDto>>> Handle(ListOrderableProductsQuery request, CancellationToken token)
    {
        return await _productQueries.ListOrderable(request.CustomerAccountId, request.SupplierId, request.PageInfo, token);
    }
}