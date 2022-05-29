using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class CancelOrder : Feature
{
    public CancelOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Cancel order with id
    /// </summary>
    [HttpPost("{id}/cancel", Name = nameof(CancelOrder))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] CancelOrderRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CancelOrderCommand(new OrderId(id), data.CancellationReason), token);
        return HandleCommandResult(result);
    }
}

public record CancelOrderRequest(string CancellationReason);
