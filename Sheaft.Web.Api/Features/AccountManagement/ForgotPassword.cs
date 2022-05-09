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

    [AllowAnonymous]
    [HttpPost("forgot")]
    public async Task<ActionResult<string>> Post([FromBody] ForgotPasswordRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<ForgotPasswordCommand>(), token);
        return HandleCommandResult(result);
    }
}

public record ForgotPasswordRequest(string Email);