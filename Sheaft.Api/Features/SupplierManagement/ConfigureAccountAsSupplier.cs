using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;
using Sheaft.Application.Models;
using Sheaft.Application.SupplierManagement;

namespace Sheaft.Api.SupplierManagement;

[Route(Routes.ACCOUNT)]
public class ConfigureAccountAsSupplier : Feature
{
    public ConfigureAccountAsSupplier(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("configure/supplier")]
    public async Task<ActionResult<string>> Post([FromBody] SupplierInfoRequest data, CancellationToken token)
    {
        var result =
            await Mediator.Execute(
                new ConfigureAccountAsSupplierCommand(data.TradeName, data.CorporateName, data.Siret, data.Email,
                    data.Phone, data.LegalAddress, data.ShippingAddress, data.BillingAddress, CurrentAccountId), token);
        
        return HandleCommandResult(result);
    }
}

public record SupplierInfoRequest(string TradeName, string CorporateName, string Siret, string Email, string Phone,
    AddressDto LegalAddress, NamedAddressDto? ShippingAddress, NamedAddressDto? BillingAddress);