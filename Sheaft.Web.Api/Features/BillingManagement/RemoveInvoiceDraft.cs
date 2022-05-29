using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class RemoveInvoiceDraft : Feature
{
    public RemoveInvoiceDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Remove invoice draft with id
    /// </summary>
    [HttpDelete("{id}", Name = nameof(RemoveInvoiceDraft))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveInvoiceDraftCommand(new InvoiceId(id)), token);
        return HandleCommandResult(result);
    }
}
