namespace Sheaft.Domain.CustomerManagement;

public interface IValidateCustomerRegistration
{
    Task<Result<bool>> CanRegisterAccount(AccountId identifier, CancellationToken token);
}