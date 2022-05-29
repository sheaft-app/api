using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class PublishOrderDraft : Feature
{
    public PublishOrderDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Publish (and send to supplier) order with id
    /// </summary>
    [HttpPost("{id}/publish", Name = nameof(PublishOrderDraft))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] PublishOrderDraftRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new PublishOrderDraftCommand(new OrderId(id), new DeliveryDate(data.DeliveryDate), data.Products), token);
        return HandleCommandResult(result);
    }
}

public record PublishOrderDraftRequest(DateTimeOffset DeliveryDate, IEnumerable<ProductQuantityDto>? Products = null);
