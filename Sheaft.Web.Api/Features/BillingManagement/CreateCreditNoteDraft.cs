using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class CreateCreditNoteDraft : Feature
{
    public CreateCreditNoteDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Create a credit note for invoice with id
    /// </summary>
    [HttpPost("{id}/credit", Name = nameof(CreateCreditNoteDraft))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreateCreditNoteDraftCommand(new InvoiceId(id)), token);
        return HandleCommandResult(result);
    }
}
