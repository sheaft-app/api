using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class AcceptOrder : Feature
{
    public AcceptOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/accept")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] AcceptOrderRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new AcceptOrderCommand(new OrderId(id), data.NewDeliveryDate.HasValue ? Maybe.From(new DeliveryDate(data.NewDeliveryDate.Value)) : Maybe<DeliveryDate>.None), token);
        return HandleCommandResult(result);
    }
}

public record AcceptOrderRequest(DateTimeOffset? NewDeliveryDate = null);
