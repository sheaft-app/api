using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

#pragma warning disable CS8604
[Route(Routes.AGREEMENTS)]
public class ListAvailableSuppliers : Feature
{
    public ListAvailableSuppliers(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List available suppliers for current user
    /// </summary>
    [HttpGet("suppliers", Name = nameof(ListAvailableSuppliers))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<AvailableSupplierDto>>> List(CancellationToken token, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListAvailableSuppliersQuery(CurrentCustomerId, PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}