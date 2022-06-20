using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BatchManagement;

[Route(Routes.SUPPLIER_BATCHES)]
public class DeleteBatch : Feature
{
    public DeleteBatch(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Remove a batch
    /// </summary>
    [HttpDelete("{batchId}", Name = nameof(DeleteBatch))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(string supplierId, string batchId, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveBatchCommand(new BatchId(batchId), new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}
