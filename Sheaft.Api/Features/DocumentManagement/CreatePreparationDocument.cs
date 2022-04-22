using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.DocumentManagement;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Api.DocumentManagement;

[Route(Routes.DOCUMENTS)]
public class CreatePreparationDocument : Feature
{
    public CreatePreparationDocument(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("preparation")]
    public async Task<ActionResult<string>> Post([FromBody] CreatePreparationDocumentRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreatePreparationDocumentCommand(data.OrderIdentifiers.Select(o => new OrderId(o)).ToList(), CurrentSupplierId, data.AutoAcceptPendingOrders ?? false), token);
        return HandleCommandResult(result);
    }
}

public record CreatePreparationDocumentRequest(IEnumerable<string> OrderIdentifiers, bool? AutoAcceptPendingOrders = false);
