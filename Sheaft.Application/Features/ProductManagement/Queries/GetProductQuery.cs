using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public record GetProductQuery(ProductId Identifier, SupplierId SupplierId) : Query<Result<ProductDetailsDto>>;

internal class GetProductHandler : IQueryHandler<GetProductQuery, Result<ProductDetailsDto>>
{
    private readonly IProductQueries _productQueries;

    public GetProductHandler(IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }
    
    public async Task<Result<ProductDetailsDto>> Handle(GetProductQuery request, CancellationToken token)
    {
        return await _productQueries.Get(request.Identifier, token);
    }
}