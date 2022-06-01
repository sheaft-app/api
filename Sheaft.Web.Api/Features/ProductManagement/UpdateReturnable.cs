using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

[Route(Routes.RETURNABLES)]
public class UpdateReturnable : Feature
{
    public UpdateReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Remove returnable with id info
    /// </summary>
    [HttpPut("{id}", Name = nameof(UpdateReturnable))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] UpdateReturnableRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateReturnableCommand(new ReturnableId(id), data.Name, data.Code,  data.UnitPrice, data.Vat), token);
        return HandleCommandResult(result);
    }
}

public record UpdateReturnableRequest(string Name, string Code, decimal UnitPrice, decimal Vat);
