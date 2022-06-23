using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIERS)]
public class CreateOrderDraft : Feature
{
    public CreateOrderDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Create a new order draft
    /// </summary>
    [HttpPost("{supplierId}/orders", Name = nameof(CreateOrderDraft))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post(string supplierId, CancellationToken token)
    {
        var result =
            await Mediator.Execute(
                new CreateOrderDraftCommand(new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}