using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Api.AgreementManagement;

[Route(Routes.CUSTOMERS)]
public class ProposeAgreementToCustomer : Feature
{
    public ProposeAgreementToCustomer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/agreement")]
    public async Task<ActionResult<string>> Post([FromRoute] string id, [FromBody] ProposeAgreementToCustomerRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new ProposeAgreementToCustomerCommand(new CustomerId(id), data.DeliveryDays, data.OrderDelayInHoursBeforeDeliveryDay, CurrentAccountId), token);
        return HandleCommandResult(result);
    }
}

public record ProposeAgreementToCustomerRequest(List<DayOfWeek> DeliveryDays, int? OrderDelayInHoursBeforeDeliveryDay);