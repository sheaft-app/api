using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

#pragma warning disable CS8604
[Route(Routes.ORDERS)]
public class CreateOrderDraft : Feature
{
    public CreateOrderDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Create a new order draft
    /// </summary>
    [HttpPost("drafts", Name = nameof(CreateOrderDraft))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromBody] CreateOrderDraftRequest data, CancellationToken token)
    {
        var result =
            await Mediator.Execute(
                new CreateOrderDraftCommand(new SupplierId(data.SupplierIdentifier), CurrentCustomerId), token);
        return HandleCommandResult(result);
    }
}

public record CreateOrderDraftRequest(string SupplierIdentifier);