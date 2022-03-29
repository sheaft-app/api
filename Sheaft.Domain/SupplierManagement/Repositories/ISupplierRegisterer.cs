namespace Sheaft.Domain.SupplierManagement;

public interface ISupplierRegisterer
{
    Task<Result<bool>> CanRegisterAccountAsSupplier(AccountId identifier, CancellationToken token);
}