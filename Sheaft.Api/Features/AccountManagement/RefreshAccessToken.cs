using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Api.AccountManagement;

[Route(Routes.TOKEN)]
public class RefreshAccessToken : Feature
{
    public RefreshAccessToken(IMediator mediator)
        : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthenticationTokenDto>> Post([FromBody] string refreshToken, CancellationToken token)
    {
        var result = await Mediator.Send(new RefreshAccessTokenCommand(refreshToken), token);
        return HandleCommandResult(result);
    }
}