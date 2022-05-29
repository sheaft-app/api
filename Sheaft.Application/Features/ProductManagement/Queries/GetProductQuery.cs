using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public record GetProductQuery(ProductId Identifier) : IQuery<Result<ProductDto>>;

internal class GetProductHandler : IQueryHandler<GetProductQuery, Result<ProductDto>>
{
    private readonly IProductQueries _productQueries;

    public GetProductHandler(IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }
    
    public async Task<Result<ProductDto>> Handle(GetProductQuery request, CancellationToken token)
    {
        return await _productQueries.Get(request.Identifier, token);
    }
}