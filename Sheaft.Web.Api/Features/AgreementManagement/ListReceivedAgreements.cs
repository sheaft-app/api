using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

#pragma warning disable CS8604
[Route(Routes.AGREEMENTS)]
public class ListReceivedAgreements : Feature
{
    public ListReceivedAgreements(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// List received agreements for current user
    /// </summary>
    [HttpGet("received", Name = nameof(ListReceivedAgreements))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResults<AgreementDto>>> List(CancellationToken token, string? search = null, int? page = 1, int? take = 10)
    {
        var result = await Mediator.Query(new ListReceivedAgreementsQuery(CurrentAccountId, PageInfo.From(page, take), search), token);
        return HandleQueryResult(result);
    }
}