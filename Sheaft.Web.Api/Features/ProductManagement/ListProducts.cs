using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.PRODUCTS)]
public class ListProducts : Feature
{
    public ListProducts(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet("")]
    public async Task<ActionResult<PaginatedResults<ProductDto>>> List(CancellationToken token, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListProductsQuery(CurrentSupplierId, PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}