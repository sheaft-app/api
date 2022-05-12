using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;
#pragma warning disable CS8618

namespace Sheaft.Web.Api.AccountManagement;

[Route(Routes.TOKEN)]
public class UserLogin : Feature
{
    public UserLogin(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Post([FromBody] LoginRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<LoginUserCommand>(), token);
        return HandleCommandResult<AuthenticationTokenDto, TokenResponse>(result);
    }
}

public record LoginRequest(string Username, string Password);