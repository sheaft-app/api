using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

[Route(Routes.AGREEMENTS)]
public class CancelAgreement : Feature
{
    public CancelAgreement(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Cancel agreement
    /// </summary>
    [HttpPut("{id}/cancel", Name = nameof(CancelAgreement))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] CancelAgreementRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CancelAgreementCommand(new AgreementId(id), data.Reason), token);
        return HandleCommandResult(result);
    }
}

public record CancelAgreementRequest(string Reason);