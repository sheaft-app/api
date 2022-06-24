using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class FulfillOrder : Feature
{
    public FulfillOrder(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Complete order with id
    /// </summary>
    [HttpPost("{id}/fulfill", Name = nameof(FulfillOrder))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] FulfillOrderRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(
            new FulfillOrdersCommand(
                new OrderId(id),
                data.DeliveryLines.Select(pb =>
                    new DeliveryLineQuantityDto(pb.ProductIdentifier, pb.Quantity, pb.BatchIdentifiers)),
                    data.NewDeliveryDate), token);
        
        return HandleCommandResult(result);
    }
}

public record FulfillOrderRequest(IEnumerable<DeliveryLineRequest> DeliveryLines, DateTimeOffset? NewDeliveryDate = null);

public record DeliveryLineRequest(string ProductIdentifier, int Quantity, IEnumerable<string> BatchIdentifiers);