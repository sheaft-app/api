using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class RefuseOrder : Feature
{
    public RefuseOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/refuse")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] RefuseOrderRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new RefuseOrderCommand(new OrderId(id), data.RefusalReason), token);
        return HandleCommandResult(result);
    }
}

public record RefuseOrderRequest(string RefusalReason);
