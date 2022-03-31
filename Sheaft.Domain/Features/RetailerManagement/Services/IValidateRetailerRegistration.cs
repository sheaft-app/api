namespace Sheaft.Domain.RetailerManagement;

public interface IValidateRetailerRegistration
{
    Task<Result<bool>> CanRegisterAccount(AccountId identifier, CancellationToken token);
}