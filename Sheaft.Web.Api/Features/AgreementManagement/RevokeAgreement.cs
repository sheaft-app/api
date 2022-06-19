using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

[Route(Routes.AGREEMENTS)]
public class RevokeAgreement : Feature
{
    public RevokeAgreement(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Revoke agreement
    /// </summary>
    [HttpPut("{id}/revoke", Name = nameof(RevokeAgreement))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] RevokeAgreementRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new RevokeAgreementCommand(new AgreementId(id), data.Reason), token);
        return HandleCommandResult(result);
    }
}

public record RevokeAgreementRequest(string Reason);