namespace Sheaft.Domain.OrderManagement;

public interface ITransformProductsToOrderLines
{
    Task<Result<IEnumerable<OrderLine>>> Transform(IEnumerable<ProductsQuantities> products, SupplierId supplierIdentifier, CancellationToken token);
}