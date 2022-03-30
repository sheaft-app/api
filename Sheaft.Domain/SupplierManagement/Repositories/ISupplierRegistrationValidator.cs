namespace Sheaft.Domain.SupplierManagement;

public interface ISupplierRegistrationValidator
{
    Task<Result<bool>> CanRegisterAccount(AccountId identifier, CancellationToken token);
}