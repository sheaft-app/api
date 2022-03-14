namespace Sheaft.Domain.AccountManagement;

public interface IValidateUniqueness
{
    Task<Result<bool>> IsUsernameAlreadyExists(Username username, CancellationToken token);
    Task<Result<bool>> IsEmailAlreadyExists(EmailAddress email, CancellationToken token);
}