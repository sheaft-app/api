using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class CreateOrderDraft : Feature
{
    public CreateOrderDraft(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("drafts")]
    public async Task<ActionResult<string>> Post([FromBody] CreateOrderDraftRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreateOrderDraftCommand(new SupplierId(data.SupplierIdentifier), CurrentCustomerId), token);
        return HandleCommandResult(result);
    }
}

public record CreateOrderDraftRequest(string SupplierIdentifier);
