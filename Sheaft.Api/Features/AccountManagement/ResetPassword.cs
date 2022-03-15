using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Api.AccountManagement;

[Route(Routes.PASSWORD)]
public class ResetPassword : Feature
{
    public ResetPassword(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("reset")]
    public async Task<ActionResult<string>> Post([FromBody] ResetPasswordDto data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<ResetPasswordCommand>(), token);
        return HandleCommandResult(result);
    }
}

public record ResetPasswordDto(string ResetToken, string Password, string Confirm);