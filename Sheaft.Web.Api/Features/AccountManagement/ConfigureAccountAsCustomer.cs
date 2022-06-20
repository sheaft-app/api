using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.CustomerManagement;

namespace Sheaft.Web.Api.AccountManagement;

#pragma warning disable CS8604
[Route(Routes.ACCOUNT)]
public class ConfigureAccountAsCustomer : Feature
{
    public ConfigureAccountAsCustomer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Configure current user account as customer profile
    /// </summary>
    [HttpPost("configure/customer", Name = nameof(ConfigureAccountAsCustomer))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromBody] CustomerInfoRequest data, CancellationToken token)
    {
        var result =
            await Mediator.Execute(
                new ConfigureAccountAsCustomerCommand(data.TradeName, data.CorporateName, data.Siret, data.Email,
                    data.Phone, data.LegalAddress, data.DeliveryAddress, data.BillingAddress, CurrentAccountId), token);
        
        return HandleCommandResult(result);
    }
}