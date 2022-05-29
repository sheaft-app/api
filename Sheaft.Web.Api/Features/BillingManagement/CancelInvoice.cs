using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BillingManagement;

[Route(Routes.INVOICES)]
public class CancelInvoice : Feature
{
    public CancelInvoice(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Cancel invoice with id
    /// </summary>
    [HttpPost("{id}/cancel", Name = nameof(CancelInvoice))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromRoute] string id, [FromBody] CancelInvoiceRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CancelInvoiceCommand(new InvoiceId(id), data.Reason), token);
        return HandleCommandResult(result);
    }
}

public record CancelInvoiceRequest(string Reason);
