namespace Sheaft.Domain.AccountManagement;

public interface IUniquenessValidator
{
    Task<Result<bool>> UsernameAlreadyExists(Username username, CancellationToken token);
    Task<Result<bool>> EmailAlreadyExists(EmailAddress email, CancellationToken token);
}