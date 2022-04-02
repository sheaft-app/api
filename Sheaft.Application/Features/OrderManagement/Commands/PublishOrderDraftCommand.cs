using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record PublishOrderDraftCommand(OrderId OrderIdentifier, OrderDeliveryDate OrderDeliveryDate, IEnumerable<ProductQuantityDto>? Products = null) : ICommand<Result>;
    
public class PublishOrderDraftHandler : ICommandHandler<PublishOrderDraftCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IGenerateOrderCode _generateOrderCode;
    private readonly ITransformProductsToOrderLines _transformProductsToOrderLines;

    public PublishOrderDraftHandler(
        IUnitOfWork uow,
        IGenerateOrderCode generateOrderCode,
        ITransformProductsToOrderLines transformProductsToOrderLines)
    {
        _uow = uow;
        _generateOrderCode = generateOrderCode;
        _transformProductsToOrderLines = transformProductsToOrderLines;
    }

    public async Task<Result> Handle(PublishOrderDraftCommand request, CancellationToken token)
    {
        var orderResult = await _uow.Orders.Get(request.OrderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        IEnumerable<OrderLine>? lines = null;

        if (request.Products != null && request.Products.Any())
        {
            var orderProducts = request.Products?
                .Select(l => new ProductsQuantities(new ProductId(l.ProductIdentifier), new Quantity(l.Quantity)))
                .ToList() ?? new List<ProductsQuantities>();
            
            var linesResult = await _transformProductsToOrderLines.Transform(orderProducts, order.SupplierIdentifier, token);
            if (linesResult.IsFailure)
                return Result.Failure(linesResult);

            lines = linesResult.Value;
        }

        var codeResult = await _generateOrderCode.GenerateNextCode(order.SupplierIdentifier, token);
        if (codeResult.IsFailure)
            return Result.Failure(codeResult);
        
        var result = order.Publish(codeResult.Value, request.OrderDeliveryDate, lines);
        if (result.IsFailure)
            return result;

        _uow.Orders.Update(order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}