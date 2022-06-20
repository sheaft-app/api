using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_RETURNABLES)]
public class ListReturnables : Feature
{
    public ListReturnables(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List available returnables for supplier
    /// </summary>
    [HttpGet("", Name = nameof(ListReturnables))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<ReturnableDto>>> List(string supplierId, CancellationToken token, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListReturnablesQuery(new SupplierId(supplierId), PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}