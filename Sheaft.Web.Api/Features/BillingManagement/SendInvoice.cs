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

    [HttpPost("{id}/send")]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new SendInvoiceCommand(new InvoiceId(id)), token);
        return HandleCommandResult(result);
    }
}
