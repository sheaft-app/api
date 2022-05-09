using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class PublishInvoiceDraft : Feature
{
    public PublishInvoiceDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/publish")]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new PublishInvoiceDraftCommand(new InvoiceId(id)), token);
        return HandleCommandResult(result);
    }
}
