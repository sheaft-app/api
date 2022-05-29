using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

[Route(Routes.RETURNABLES)]
public class DeleteReturnable : Feature
{
    public DeleteReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Remove returnable with id
    /// </summary>
    [HttpDelete("{id}", Name = nameof(DeleteReturnable))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveReturnableCommand(new ReturnableId(id)), token);
        return HandleCommandResult(result);
    }
}
