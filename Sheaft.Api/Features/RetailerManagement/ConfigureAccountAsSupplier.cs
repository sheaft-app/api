using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.Models;
using Sheaft.Application.RetailerManagement;

namespace Sheaft.Api.RetailerManagement;

[Route(Routes.PROFILES)]
public class ConfigureAccountAsRetailer : Feature
{
    public ConfigureAccountAsRetailer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("configure/retailer")]
    public async Task<ActionResult<string>> Post([FromBody] RetailerInfoRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new ConfigureAccountAsRetailerCommand(data.TradeName, data.CorporateName, data.Siret, data.Email, data.Phone, data.LegalAddress, data.DeliveryAddress, CurrentAccountId), token);
        return HandleCommandResult(result);
    }
}

public record RetailerInfoRequest(string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? DeliveryAddress);
