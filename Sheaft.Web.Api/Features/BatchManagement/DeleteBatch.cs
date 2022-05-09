using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BatchManagement;

[Route(Routes.BATCHES)]
public class DeleteBatch : Feature
{
    public DeleteBatch(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveBatchCommand(new BatchId(id)), token);
        return HandleCommandResult(result);
    }
}
