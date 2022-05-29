using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BillingManagement;

[Route(Routes.DELIVERIES)]
public class CreateInvoiceForDelivery : Feature
{
    public CreateInvoiceForDelivery(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Create an invoice for delivery with id
    /// </summary>
    [HttpPost("{id}/invoice", Name = nameof(CreateInvoiceForDelivery))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreateInvoiceForDeliveryCommand(new DeliveryId(id)), token);
        return HandleCommandResult(result);
    }
}
