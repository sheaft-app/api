using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

[Route(Routes.SUPPLIER_RETURNABLES)]
public class UpdateReturnable : Feature
{
    public UpdateReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Remove returnable with id info
    /// </summary>
    [HttpPut("{returnableId}", Name = nameof(UpdateReturnable))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(string supplierId, string returnableId, UpdateReturnableRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateReturnableCommand(new SupplierId(supplierId), new ReturnableId(returnableId), data.Name, data.Code,  data.UnitPrice, data.Vat), token);
        return HandleCommandResult(result);
    }
}

public record UpdateReturnableRequest(string Name, string Code, decimal UnitPrice, decimal Vat);
