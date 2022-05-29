using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Web.Api.AccountManagement;

[Route(Routes.PASSWORD)]
public class ForgotPassword : Feature
{
    public ForgotPassword(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Generate and send a reset link on user email
    /// </summary>
    [AllowAnonymous]
    [HttpPost("forgot", Name = nameof(ForgotPassword))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromBody] ForgotPasswordRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<ForgotPasswordCommand>(), token);
        return HandleCommandResult(result);
    }
}

public record ForgotPasswordRequest(string Email);