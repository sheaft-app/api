using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Api.ProductManagement;

[Route(Routes.RETURNABLES)]
public class DeleteReturnable : Feature
{
    public DeleteReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveReturnableCommand(new ReturnableId(id)), token);
        return HandleCommandResult(result);
    }
}
