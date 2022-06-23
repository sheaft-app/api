using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

#pragma warning disable CS8604
[Route(Routes.ORDERS)]
public class ListOrders : Feature
{
    public ListOrders(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List available orders for current user
    /// </summary>
    [HttpGet("", Name = nameof(ListOrders))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<OrderDto>>> List(CancellationToken token, [FromQuery]IEnumerable<OrderStatus>? statuses = null, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListOrdersQuery(statuses, PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}