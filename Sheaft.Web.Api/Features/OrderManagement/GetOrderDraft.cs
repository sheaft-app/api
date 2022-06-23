using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

#pragma warning disable CS8604
[Route(Routes.ORDERS)]
public class GetOrderDraft : Feature
{
    public GetOrderDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve order draft with id
    /// </summary>
    [HttpGet("draft/{orderId}", Name = nameof(GetOrderDraft))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDraftDto>> Get(string orderId, CancellationToken token)
    {
        var result = await Mediator.Query(new GetOrderDraftQuery(new OrderId(orderId)), token);
        return HandleCommandResult(result);
    }
}