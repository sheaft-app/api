using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class CancelOrder : Feature
{
    public CancelOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] CancelOrderRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CancelOrderCommand(new OrderId(id), data.CancellationReason), token);
        return HandleCommandResult(result);
    }
}

public record CancelOrderRequest(string CancellationReason);
