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
        var result = await Mediator.Execute(
            new FulfillOrdersCommand(
                new OrderId(id),
                data.DeliveryLines.Select(pb =>
                    new DeliveryLineDto(pb.ProductIdentifier, pb.Quantity, pb.BatchIdentifiers)),
                    data.NewDeliveryDate), token);
        
        return HandleCommandResult(result);
    }
}

public record FulfillOrderRequest(IEnumerable<DeliveryLineRequest> DeliveryLines, DateTimeOffset? NewDeliveryDate = null);

public record DeliveryLineRequest(string ProductIdentifier, int Quantity, IEnumerable<string> BatchIdentifiers);