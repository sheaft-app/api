using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.Models;
using Sheaft.Application.CustomerManagement;

namespace Sheaft.Api.CustomerManagement;

[Route(Routes.ACCOUNT)]
public class ConfigureAccountAsCustomer : Feature
{
    public ConfigureAccountAsCustomer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("configure/customer")]
    public async Task<ActionResult<string>> Post([FromBody] CustomerInfoRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new ConfigureAccountAsCustomerCommand(data.TradeName, data.CorporateName, data.Siret, data.Email, data.Phone, data.LegalAddress, data.DeliveryAddress, CurrentAccountId), token);
        return HandleCommandResult(result);
    }
}

public record CustomerInfoRequest(string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? DeliveryAddress);
