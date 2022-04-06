using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Api.OrderManagement;

[Route(Routes.DELIVERIES)]
public class DeliverOrder : Feature
{
    public DeliverOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/deliver")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] DeliverOrdersRequest? data, CancellationToken token)
    {
        var result = await Mediator.Execute(
            new DeliverOrderCommand(new DeliveryId(id),
                data?.ProductsAdjustments?.Select(p =>
                    new ProductAdjustment(new ProductId(p.Identifier), new Quantity(p.Quantity))),
                data?.ReturnedReturnables?.Select(p =>
                    new ReturnedReturnable(new ReturnableId(p.Identifier), new Quantity(p.Quantity)))), token);
        
        return HandleCommandResult(result);
    }
}

public record DeliverOrdersRequest(IEnumerable<LineAdjustmentRequest>? ProductsAdjustments = null,
    IEnumerable<LineAdjustmentRequest>? ReturnedReturnables = null);

public record LineAdjustmentRequest(string Identifier, int Quantity);