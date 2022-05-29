using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.PRODUCTS)]
public class GetProduct : Feature
{
    public GetProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> Get([FromQuery] string id, CancellationToken token)
    {
        var result = await Mediator.Query(new GetProductQuery(new ProductId(id)), token);
        return HandleCommandResult(result);
    }
}