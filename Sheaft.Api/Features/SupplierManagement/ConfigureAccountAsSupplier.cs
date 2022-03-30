using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;
using Sheaft.Application.SupplierManagement;

namespace Sheaft.Api.SupplierManagement;

[Route(Routes.SUPPLIERS)]
public class ConfigureAccountAsSupplier : Feature
{
    public ConfigureAccountAsSupplier(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("configure")]
    public async Task<ActionResult<string>> Post([FromBody] SupplierInfoRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new ConfigureAccountAsSupplierCommand(data.TradeName, data.CorporateName, data.Siret, data.Email, data.Phone, data.LegalAddress, data.ShippingAddress, CurrentUserId), token);
        return HandleCommandResult(result);
    }
}

public record SupplierInfoRequest(string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? ShippingAddress);
