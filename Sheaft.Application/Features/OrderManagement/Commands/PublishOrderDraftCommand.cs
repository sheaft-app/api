using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record PublishOrderDraftCommand(OrderId OrderIdentifier, DeliveryDate DeliveryDate, IEnumerable<ProductQuantityDto>? Products = null) : Command<Result>;
    
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
        var orderProducts = request.Products?
            .Select(l => new ProductsQuantities(new ProductId(l.ProductIdentifier), new OrderedQuantity(l.Quantity)))
            .ToList() ?? new List<ProductsQuantities>();
        
        var result = await _publishOrders.Publish(request.OrderIdentifier, request.DeliveryDate, orderProducts, token);
        if (result.IsFailure)
            return result;
        
        return await _uow.Save(token);
    }
}