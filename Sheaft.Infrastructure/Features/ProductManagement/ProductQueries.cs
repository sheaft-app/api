using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.ProductManagement;

public class ProductQueries : IProductQueries
{
    public Task<Result<ProductDto>> Get(ProductId identifier, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}