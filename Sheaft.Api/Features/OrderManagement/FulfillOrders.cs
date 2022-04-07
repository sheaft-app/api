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
        var result = await Mediator.Execute(
            new FulfillOrdersCommand(
                data.OrderIdentifiers.Select(o => new OrderId(o)),
                data.RegroupOrders,
                data.NewDeliveryDates?.Select(nd =>
                    new CustomerDeliveryDate(new CustomerId(nd.CustomerIdentifier),
                        new DeliveryDate(nd.NewDeliveryDate))) ?? new List<CustomerDeliveryDate>(),
                data.ProductsBatches?.Select(pb =>
                    new ProductBatches(new ProductId(pb.ProductIdentifier),
                        pb.BatchIdentifiers.Select(b => new BatchId(b)))) ?? new List<ProductBatches>()), token);
        return HandleCommandResult(result);
    }
}

public record FulfillOrdersRequest(IEnumerable<string> OrderIdentifiers, bool RegroupOrders,
    IEnumerable<CustomerDeliveryDateRequest>? NewDeliveryDates = null,
    IEnumerable<ProductBatchesRequest>? ProductsBatches = null);

public record CustomerDeliveryDateRequest(string CustomerIdentifier, DateTimeOffset NewDeliveryDate);

public record ProductBatchesRequest(string ProductIdentifier, IEnumerable<string> BatchIdentifiers);