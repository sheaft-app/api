using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public record ListProductsQuery(SupplierId SupplierId, PageInfo PageInfo) : Query<Result<PagedResult<ProductDto>>>;

internal class ListProductsHandler : IQueryHandler<ListProductsQuery, Result<PagedResult<ProductDto>>>
{
    private readonly IProductQueries _productQueries;

    public ListProductsHandler(IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }
    
    public async Task<Result<PagedResult<ProductDto>>> Handle(ListProductsQuery request, CancellationToken token)
    {
        return await _productQueries.List(request.SupplierId, request.PageInfo, token);
    }
}