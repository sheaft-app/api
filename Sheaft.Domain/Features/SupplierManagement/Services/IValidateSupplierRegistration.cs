namespace Sheaft.Domain.SupplierManagement;

public interface IValidateSupplierRegistration
{
    Task<Result<bool>> CanRegisterAccount(AccountId identifier, CancellationToken token);
}