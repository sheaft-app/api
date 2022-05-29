using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Web.Api.AccountManagement;

[Route(Routes.TOKEN)]
public class RefreshAccessToken : Feature
{
    public RefreshAccessToken(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Regenerate an access_token for current user
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh", Name = nameof(RefreshAccessToken))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponse>> Post([FromBody] RefreshTokenRequest refreshToken, CancellationToken token)
    {
        var result = await Mediator.Execute(new RefreshAccessTokenCommand(refreshToken.Token), token);
        return HandleCommandResult<AuthenticationTokenDto, TokenResponse>(result);
    }
}

public record RefreshTokenRequest(string Token);