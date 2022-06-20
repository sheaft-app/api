using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_PRODUCTS)]
public class GetProduct : Feature
{
    public GetProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve product with id
    /// </summary>
    [HttpGet("{productId}", Name = nameof(GetProduct))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDetailsDto>> Get(string supplierId, string productId, CancellationToken token)
    {
        var result = await Mediator.Query(new GetProductQuery(new ProductId(productId), new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}