namespace Sheaft.Domain.SupplierManagement;

public interface ISupplierRepository : IRepository<Supplier, SupplierId>
{
    Task<Result<Supplier>> Get(AccountId identifier, CancellationToken token);
}