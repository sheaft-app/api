using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record UpdateOrderDraftProductsCommand(OrderId OrderIdentifier, IEnumerable<ProductQuantityDto> Products) : ICommand<Result>;
    
public class UpdateOrderDraftProductsHandler : ICommandHandler<UpdateOrderDraftProductsCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly ITransformProductsToOrderLines _transformProductsToOrderLines;

    public UpdateOrderDraftProductsHandler(
        IUnitOfWork uow,
        ITransformProductsToOrderLines transformProductsToOrderLines)
    {
        _uow = uow;
        _transformProductsToOrderLines = transformProductsToOrderLines;
    }

    public async Task<Result> Handle(UpdateOrderDraftProductsCommand request, CancellationToken token)
    {
        var orderResult = await _uow.Orders.Get(request.OrderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        var orderProducts = request.Products?
            .Select(l => new ProductsQuantities(new ProductId(l.ProductIdentifier), new OrderedQuantity(l.Quantity)))
            .ToList() ?? new List<ProductsQuantities>();
        
        var linesResult = await _transformProductsToOrderLines.Transform(orderProducts, order.SupplierId, token);
        if (linesResult.IsFailure)
            return Result.Failure(linesResult);
        
        var result = order.UpdateDraftLines(linesResult.Value);
        if (result.IsFailure)
            return result;

        _uow.Orders.Update(order);
        return await _uow.Save(token);
    }
}