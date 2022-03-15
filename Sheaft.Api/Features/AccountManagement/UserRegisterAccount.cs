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
        var (tradeName, contactEmail, contactPhone, (commercialName, siret, legalAddress), user) = data;
        var result = await Mediator.Send(
            new RegisterAccountCommand(user.Email, user.Password, user.Confirm, tradeName, contactEmail, contactPhone,
                commercialName, siret, legalAddress.Line1, legalAddress.Line2, legalAddress.Zipcode,
                legalAddress.City, user.Firstname, user.Lastname), token);
        
        return HandleCommandResult(result);
    }
}

public record RegisterDto(string TradeName, string ContactEmail, string ContactPhone, RegisterLegalDto Legal,
    RegisterUserDto User);

public record RegisterLegalDto(string CommercialName, string Siret, LegalAddressDto Address);

public record LegalAddressDto(string Line1, string Line2, string Zipcode, string City);

public record RegisterUserDto(string Firstname, string Lastname, string Email, string Password, string Confirm);