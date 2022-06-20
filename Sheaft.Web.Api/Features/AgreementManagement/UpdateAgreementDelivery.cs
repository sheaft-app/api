using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

[Route(Routes.AGREEMENTS)]
public class UpdateAgreementDelivery : Feature
{
    public UpdateAgreementDelivery(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// update agreement delivery
    /// </summary>
    [HttpPut("{id}", Name = nameof(UpdateAgreementDelivery))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] UpdateAgreementDeliveryRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateAgreementDeliveryCommand(new AgreementId(id), data.DeliveryDays, data.LimitOrderHourOffset), token);
        return HandleCommandResult(result);
    }
}

public record UpdateAgreementDeliveryRequest(List<DayOfWeek> DeliveryDays, int LimitOrderHourOffset);