using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record PublishOrderDraftCommand(OrderId OrderIdentifier, OrderDeliveryDate OrderDeliveryDate, IEnumerable<ProductQuantityDto>? Products = null) : ICommand<Result>;
    
public class PublishOrderDraftHandler : ICommandHandler<PublishOrderDraftCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IPublishOrders _publishOrders;

    public PublishOrderDraftHandler(
        IUnitOfWork uow,
        IPublishOrders publishOrders)
    {
        _uow = uow;
        _publishOrders = publishOrders;
    }

    public async Task<Result> Handle(PublishOrderDraftCommand request, CancellationToken token)
    {
        var orderResult = await _uow.Orders.Get(request.OrderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        
        var orderProducts = request.Products?
            .Select(l => new ProductsQuantities(new ProductId(l.ProductIdentifier), new Quantity(l.Quantity)))
            .ToList() ?? new List<ProductsQuantities>();
        
        var orderDeliveryDate = request.OrderDeliveryDate;
        
        var result = await _publishOrders.Publish(order, orderDeliveryDate, orderProducts, token);
        if (result.IsFailure)
            return result;

        _uow.Orders.Update(order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}