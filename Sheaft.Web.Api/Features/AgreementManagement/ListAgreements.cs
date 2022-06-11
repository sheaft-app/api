using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

#pragma warning disable CS8604
[Route(Routes.AGREEMENTS)]
public class ListAgreements : Feature
{
    public ListAgreements(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List available agreements for current user
    /// </summary>
    [HttpGet("", Name = nameof(ListAgreements))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<AgreementDto>>> List(CancellationToken token, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListAgreementsQuery(CurrentAccountId, PageInfo.From(page, take)), token);
        return HandleQueryResult(result);
    }
}