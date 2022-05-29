using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.DocumentManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.DocumentManagement;

#pragma warning disable CS8604
[Route(Routes.DOCUMENTS)]
public class CreatePreparationDocument : Feature
{
    public CreatePreparationDocument(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Create a preparation document for specfified orders
    /// </summary>
    [HttpPost("preparation", Name = nameof(CreatePreparationDocument))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromBody] CreatePreparationDocumentRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreatePreparationDocumentCommand(data.OrderIdentifiers.Select(o => new OrderId(o)).ToList(), new OwnerId(CurrentSupplierId), data.AutoAcceptPendingOrders ?? false), token);
        return HandleCommandResult(result);
    }
}

public record CreatePreparationDocumentRequest(IEnumerable<string> OrderIdentifiers, bool? AutoAcceptPendingOrders = false);
