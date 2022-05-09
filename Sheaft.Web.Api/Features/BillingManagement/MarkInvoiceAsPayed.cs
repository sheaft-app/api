using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class SendInvoiceDraft : Feature
{
    public SendInvoiceDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/payed")]
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