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

    /// <summary>
    /// Remove a batch
    /// </summary>
    [HttpDelete("{id}", Name = nameof(DeleteBatch))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveBatchCommand(new BatchId(id)), token);
        return HandleCommandResult(result);
    }
}
