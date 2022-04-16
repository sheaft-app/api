using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class RemoveInvoiceDraft : Feature
{
    public RemoveInvoiceDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveInvoiceDraftCommand(new InvoiceId(id)), token);
        return HandleCommandResult(result);
    }
}
