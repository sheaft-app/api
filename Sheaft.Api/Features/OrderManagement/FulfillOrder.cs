using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class FulfillOrder : Feature
{
    public FulfillOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/fulfill")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] FulfillOrderRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new FulfillOrderCommand(new OrderId(id), data.NewDeliveryDate.HasValue ? Maybe.From(new DeliveryDate(data.NewDeliveryDate.Value)) : Maybe<DeliveryDate>.None), token);
        return HandleCommandResult(result);
    }
}

public record FulfillOrderRequest(DateTimeOffset? NewDeliveryDate = null);
