using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public record ListOrderableProductsQuery(SupplierId SupplierId, PageInfo PageInfo) : Query<Result<PagedResult<OrderableProductDto>>>;

internal class ListOrderableProductsHandler : IQueryHandler<ListOrderableProductsQuery, Result<PagedResult<OrderableProductDto>>>
{
    private readonly IProductQueries _productQueries;

    public ListOrderableProductsHandler(IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }
    
    public async Task<Result<PagedResult<OrderableProductDto>>> Handle(ListOrderableProductsQuery request, CancellationToken token)
    {
        return await _productQueries.ListOrderable(request.RequestUser.AccountId, request.SupplierId, request.PageInfo, token);
    }
}