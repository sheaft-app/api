using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class FulfillOrders : Feature
{
    public FulfillOrders(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("/fulfill")]
    public async Task<ActionResult> Post([FromBody] FulfillOrdersRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new FulfillOrdersCommand(data.OrderIdentifiers.Select(o => new OrderId(o)), data.NewDeliveryDates?.Select(nd => new CustomerDeliveryDate(new CustomerId(nd.CustomerIdentifier), new DeliveryDate(nd.NewDeliveryDate))) ?? new List<CustomerDeliveryDate>()), token);
        return HandleCommandResult(result);
    }
}

public record FulfillOrdersRequest(IEnumerable<string> OrderIdentifiers, IEnumerable<CustomerDeliveryDateRequest>? NewDeliveryDates = null);
public record CustomerDeliveryDateRequest(string CustomerIdentifier, DateTimeOffset NewDeliveryDate);
