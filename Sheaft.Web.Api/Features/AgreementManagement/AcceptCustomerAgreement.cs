using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

[Route(Routes.AGREEMENTS)]
public class AcceptCustomerAgreement : Feature
{
    public AcceptCustomerAgreement(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Accept customer agreement
    /// </summary>
    [HttpPut("{id}/accept/customer", Name = nameof(AcceptCustomerAgreement))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] AcceptCustomerAgreementRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new AcceptCustomerAgreementCommand(new AgreementId(id), data.DeliveryDays, data.OrderDelayInHoursBeforeDeliveryDay), token);
        return HandleCommandResult(result);
    }
}

public record AcceptCustomerAgreementRequest(List<DayOfWeek> DeliveryDays, int OrderDelayInHoursBeforeDeliveryDay);