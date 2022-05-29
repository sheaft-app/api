using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.Models;
using Sheaft.Application.SupplierManagement;

namespace Sheaft.Web.Api.SupplierManagement;

#pragma warning disable CS8604
[Route(Routes.ACCOUNT)]
public class ConfigureAccountAsSupplier : Feature
{
    public ConfigureAccountAsSupplier(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Configure user account as supplier profile
    /// </summary>
    [HttpPost("configure/supplier", Name = nameof(ConfigureAccountAsSupplier))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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