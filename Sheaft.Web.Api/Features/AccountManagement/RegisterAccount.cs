using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Web.Api.AccountManagement;

[Route(Routes.API)]
public class RegisterAccount : Feature
{
    public RegisterAccount(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<string>> Post([FromBody] RegisterRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new RegisterAccountCommand(data.Email, data.Password, data.Confirm, data.Firstname, data.Lastname), token);
        return HandleCommandResult(result);
    }
}

public record RegisterRequest(string Email, string Password, string Confirm, string Firstname, string Lastname);
