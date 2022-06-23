using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.SupplierManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.SupplierManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIERS)]
public class ListAvailableSuppliers : Feature
{
    public ListAvailableSuppliers(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List available suppliers for current user
    /// </summary>
    [HttpGet("", Name = nameof(ListAvailableSuppliers))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<AvailableSupplierDto>>> List(CancellationToken token, string? search = null, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListAvailableSuppliersQuery(PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}