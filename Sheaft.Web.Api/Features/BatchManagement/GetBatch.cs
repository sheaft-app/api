using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BatchManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_BATCHES)]
public class GetBatch : Feature
{
    public GetBatch(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve batch with id
    /// </summary>
    [HttpGet("{batchId}", Name = nameof(GetBatch))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BatchDto>> Get(string supplierId, string batchId, CancellationToken token)
    {
        var result = await Mediator.Query(new GetBatchQuery(new BatchId(batchId), new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}