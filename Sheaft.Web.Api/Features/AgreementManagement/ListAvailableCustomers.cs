using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

#pragma warning disable CS8604
[Route(Routes.AGREEMENTS)]
public class ListAvailableCustomers : Feature
{
    public ListAvailableCustomers(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List available customers for current user
    /// </summary>
    [HttpGet("customers", Name = nameof(ListAvailableCustomers))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<AvailableCustomerDto>>> List(CancellationToken token, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListAvailableCustomersQuery(CurrentSupplierId, PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}