using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIERS)]
public class GetNextSupplierDeliveryDates : Feature
{
    public GetNextSupplierDeliveryDates(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Get supplier next delivery dates for current user
    /// </summary>
    [HttpGet("{supplierId}/next-delivery-dates", Name = nameof(GetNextSupplierDeliveryDates))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<DateTime>>> Get(string supplierId, CancellationToken token)
    {
        var result = await Mediator.Query(new GetNextSupplierDeliveryDatesQuery(new SupplierId(supplierId), CurrentAccountId), token);
        return HandleCommandResult(result);
    }
}