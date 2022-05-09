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

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthenticationTokenDto>> Post([FromBody] string refreshToken, CancellationToken token)
    {
        var result = await Mediator.Execute(new RefreshAccessTokenCommand(refreshToken), token);
        return HandleCommandResult(result);
    }
}