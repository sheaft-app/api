using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record FulfillOrdersCommand(OrderId OrderIdentifier, IEnumerable<DeliveryLineDto> DeliveryLines,
    DateTimeOffset? NewDeliveryDate) : Command<Result>;

public class FulfillOrdersHandler : ICommandHandler<FulfillOrdersCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IFulfillOrders _fulfillOrders;

    public FulfillOrdersHandler(IUnitOfWork uow,
        IFulfillOrders fulfillOrders)
    {
        _uow = uow;
        _fulfillOrders = fulfillOrders;
    }

    public async Task<Result> Handle(FulfillOrdersCommand request, CancellationToken token)
    {
        var result = await _fulfillOrders.Fulfill(request.OrderIdentifier, request.DeliveryLines.Select(
                dl => new DeliveryProductBatches(new ProductId(dl.ProductIdentifier), new Quantity(dl.Quantity),
                    dl.BatchIdentifiers?.Select(b => new BatchId(b)) ?? new List<BatchId>())),
            request.NewDeliveryDate != null ? new DeliveryDate(request.NewDeliveryDate.Value) : null,
            request.CreatedAt, token);
        
        if (result.IsFailure)
            return Result.Failure(result);

        return await _uow.Save(token);
    }
}