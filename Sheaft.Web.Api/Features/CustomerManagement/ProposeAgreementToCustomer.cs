using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.CustomerManagement;

#pragma warning disable CS8604
[Route(Routes.CUSTOMERS)]
public class ProposeAgreementToCustomer : Feature
{
    public ProposeAgreementToCustomer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Send an agreement to customer
    /// </summary>
    [HttpPost("{id}/agreement", Name = nameof(ProposeAgreementToCustomer))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromRoute] string id, [FromBody] ProposeAgreementToCustomerRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new ProposeAgreementToCustomerCommand(new CustomerId(id), data.DeliveryDays, data.OrderDelayInHoursBeforeDeliveryDay), token);
        return HandleCommandResult(result);
    }
}

public record ProposeAgreementToCustomerRequest(List<DayOfWeek> DeliveryDays, int? OrderDelayInHoursBeforeDeliveryDay);