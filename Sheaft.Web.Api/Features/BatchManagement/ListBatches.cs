using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BatchManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_BATCHES)]
public class ListBatches : Feature
{
    public ListBatches(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List available batches for supplier
    /// </summary>
    [HttpGet("", Name = nameof(ListBatches))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<BatchDto>>> List(string supplierId, CancellationToken token, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListBatchesQuery(new SupplierId(supplierId), PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}