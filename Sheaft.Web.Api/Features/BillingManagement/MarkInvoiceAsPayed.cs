using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class MarkInvoiceAsPayed : Feature
{
    public MarkInvoiceAsPayed(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Mark invoice with id as payed
    /// </summary>
    [HttpPost("{id}/payed", Name = nameof(MarkInvoiceAsPayed))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, MarkInvoiceAsPayedRequest data, CancellationToken token)
    {
        var result =
            await Mediator.Execute(
                new MarkInvoiceAsPayedCommand(new InvoiceId(id), data.Reference, data.PayedOn, data.PaymentKind),
                token);
        
        return HandleCommandResult(result);
    }
}

public record MarkInvoiceAsPayedRequest(string Reference, DateTimeOffset PayedOn, PaymentKind PaymentKind);