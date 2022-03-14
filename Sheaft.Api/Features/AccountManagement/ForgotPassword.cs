using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Api.AccountManagement;

[Route(Routes.PASSWORD)]
public class ForgotPassword : Feature
{
    public ForgotPassword(IMediator mediator)
        : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("forgot")]
    public async Task<ActionResult<string>> Post([FromBody] ForgotPasswordDto data, CancellationToken token)
    {
        var result = await Mediator.Send(data.Adapt<ForgotPasswordCommand>(), token);
        return HandleCommandResult(result);
    }
}

public record ForgotPasswordDto(string Email);