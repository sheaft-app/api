using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

[Route(Routes.AGREEMENTS)]
public class AcceptAgreement : Feature
{
    public AcceptAgreement(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPut("{id}/accept")]
    public async Task<ActionResult<string>> Post([FromRoute] string id, [FromBody] AcceptAgreementRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new AcceptAgreementCommand(new AgreementId(id), data.DeliveryDays, data.OrderDelayInHoursBeforeDeliveryDay), token);
        return HandleCommandResult(result);
    }
}

public record AcceptAgreementRequest(List<DayOfWeek>? DeliveryDays = null, int? OrderDelayInHoursBeforeDeliveryDay = null);