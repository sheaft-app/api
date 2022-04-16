using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class CancelInvoice : Feature
{
    public CancelInvoice(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<string>> Post([FromRoute] string id, [FromBody] CancelInvoiceRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CancelInvoiceCommand(new InvoiceId(id), data.Reason), token);
        return HandleCommandResult(result);
    }
}

public record CancelInvoiceRequest(string Reason);
