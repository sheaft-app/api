using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Api.AccountManagement;

[Route(Routes.API)]
public class UserRegisterAccount : Feature
{
    public UserRegisterAccount(IMediator mediator)
        : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<string>> Post([FromBody] RegisterDto data, CancellationToken token)
    {
        var result = await Mediator.Send(data.Adapt<RegisterAccountCommand>(), token);
        return HandleCommandResult(result);
    }
}

public record RegisterDto(string Email, string Firstname, string Lastname, string Password, string Confirm);