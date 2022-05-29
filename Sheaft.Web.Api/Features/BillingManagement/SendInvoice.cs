using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class SendInvoice : Feature
{
    public SendInvoice(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Send invoice with id to customer
    /// </summary>
    [HttpPost("{id}/send", Name = nameof(SendInvoice))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new SendInvoiceCommand(new InvoiceId(id)), token);
        return HandleCommandResult(result);
    }
}
