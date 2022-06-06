namespace Sheaft.Domain.ProductManagement;

public interface IReturnableRepository : IRepository<Returnable, ReturnableId>
{
    Task<Result<Maybe<Returnable>>> Find(ReturnableId? identifier, CancellationToken token);
    Task<Result<Maybe<Returnable>>> Find(ReturnableReference reference, SupplierId supplierIdentifier, CancellationToken token);
}