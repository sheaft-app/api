namespace Sheaft.Domain.ProductManagement;

public interface IReturnableRepository : IRepository<Returnable, ReturnableId>
{
    Task<Result<Maybe<Returnable>>> FindWithReference(ReturnableReference reference, SupplierId supplierIdentifier,
        CancellationToken token);
}