using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

#pragma warning disable CS8604
[Route(Routes.ORDERS)]
public class GetOrder : Feature
{
    public GetOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve order with id
    /// </summary>
    [HttpGet("{orderId}", Name = nameof(GetOrder))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDetailsDto>> Get(string orderId, CancellationToken token)
    {
        var result = await Mediator.Query(new GetOrderQuery(new OrderId(orderId)), token);
        return HandleCommandResult(result);
    }
}