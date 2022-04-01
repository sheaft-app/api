namespace Sheaft.Domain;

public interface IValidateCustomerRegistration
{
    Task<Result<bool>> CanRegisterAccount(AccountId identifier, CancellationToken token);
}