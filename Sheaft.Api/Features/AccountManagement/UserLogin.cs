using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Api.AccountManagement;

[Route(Routes.TOKEN)]
public class UserLogin : Feature
{
    public UserLogin(IMediator mediator)
        : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationTokenDto>> Post([FromBody] LoginDto data, CancellationToken token)
    {
        var result = await Mediator.Send(data.Adapt<LoginUserCommand>(), token);
        return HandleCommandResult(result);
    }
}

public record LoginDto(string Username, string Password);