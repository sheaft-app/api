using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

[Route(Routes.AGREEMENTS)]
public class RefuseAgreement : Feature
{
    public RefuseAgreement(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Refuse agreement
    /// </summary>
    [HttpPut("{id}/refuse", Name = nameof(RefuseAgreement))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] RefuseAgreementRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new RefuseAgreementCommand(new AgreementId(id), data.Reason), token);
        return HandleCommandResult(result);
    }
}

public record RefuseAgreementRequest(string Reason);