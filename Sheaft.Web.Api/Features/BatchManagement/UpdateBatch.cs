using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BatchManagement;

[Route(Routes.SUPPLIER_BATCHES)]
public class UpdateBatch : Feature
{
    public UpdateBatch(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Update a batch
    /// </summary>
    [HttpPut("{batchId}", Name = nameof(UpdateBatch))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(string supplierId, string batchId, UpdateBatchRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateBatchCommand(new SupplierId(supplierId), new BatchId(batchId), data.Number, data.DateKind,  data.Date), token);
        return HandleCommandResult(result);
    }
}

public record UpdateBatchRequest(string Number, BatchDateKind DateKind, DateOnly Date);
