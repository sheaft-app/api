using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;
#pragma warning disable CS8618

namespace Sheaft.Web.Api.AccountManagement;

[Route(Routes.TOKEN)]
public class LoginUser : Feature
{
    public LoginUser(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Log the user in and generate access token / refresh token
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login", Name = nameof(LoginUser))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponse>> Post([FromBody] LoginRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new LoginUserCommand(data.Username, data.Password), token);
        return HandleCommandResult<AuthenticationTokenDto, TokenResponse>(result);
    }
}

public record LoginRequest(string Username, string Password);