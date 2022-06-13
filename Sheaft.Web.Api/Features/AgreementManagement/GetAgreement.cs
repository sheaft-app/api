using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

#pragma warning disable CS8604
[Route(Routes.AGREEMENTS)]
public class GetAgreement : Feature
{
    public GetAgreement(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve agreement with id
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetAgreement))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgreementDetailsDto>> Get(string id, CancellationToken token)
    {
        var result = await Mediator.Query(new GetAgreementQuery(new AgreementId(id)), token);
        return HandleCommandResult(result);
    }
}