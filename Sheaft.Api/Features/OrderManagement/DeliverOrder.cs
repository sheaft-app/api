using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Api.OrderManagement;

[Route(Routes.DELIVERIES)]
public class DeliverOrder : Feature
{
    public DeliverOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/deliver")]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new DeliverOrderCommand(new DeliveryId(id)), token);
        return HandleCommandResult(result);
    }
}
