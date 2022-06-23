using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_PRODUCTS)]
public class ListOrderableProducts : Feature
{
    public ListOrderableProducts(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List supplier orderable products for current user
    /// </summary>
    [HttpGet("orderable", Name = nameof(ListOrderableProducts))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<OrderableProductDto>>> List(string supplierId, CancellationToken token, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListOrderableProductsQuery(new SupplierId(supplierId), PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}