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
            .Select(l => new ProductsQuantities(new ProductId(l.ProductIdentifier), new Quantity(l.Quantity)))
            .ToList() ?? new List<ProductsQuantities>();
        
        var linesResult = await _transformProductsToOrderLines.Transform(orderProducts, order.SupplierIdentifier, token);
        if (linesResult.IsFailure)
            return Result.Failure(linesResult);
        
        order.UpdateDraftLines(linesResult.Value);

        _uow.Orders.Update(order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}