using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Web.Api.AccountManagement;

[Route(Routes.PASSWORD)]
public class ResetPassword : Feature
{
    public ResetPassword(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Reset the password with the token retrieved from email link
    /// </summary>
    [AllowAnonymous]
    [HttpPost("reset", Name = nameof(ResetPassword))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromBody] ResetPasswordRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<ResetPasswordCommand>(), token);
        return HandleCommandResult(result);
    }
}

public record ResetPasswordRequest(string ResetToken, string Password, string Confirm);