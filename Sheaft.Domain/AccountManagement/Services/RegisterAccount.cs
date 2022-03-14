using Sheaft.Domain.ProfileManagement;

namespace Sheaft.Domain.AccountManagement;

public interface ICreateAccount
{
    Task<Result<Account>> Create(Username username, EmailAddress email, NewPassword password, Profile profile, CancellationToken token);
}

internal class CreateAccount : ICreateAccount
{
    private readonly IValidateUniqueness _validateUniqueness;
    private readonly IPasswordHasher _passwordHasher;

    public CreateAccount(IValidateUniqueness validateUniqueness, IPasswordHasher passwordHasher)
    {
        _validateUniqueness = validateUniqueness;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Account>> Create(Username username, EmailAddress email, NewPassword password, Profile profile, CancellationToken token)
    {
        var usernameIsUniqueResult = await _validateUniqueness.IsUsernameAlreadyExists(username, token);
        if (usernameIsUniqueResult.IsSuccess && usernameIsUniqueResult.Value)
            return Result.Failure<Account>(ErrorKind.Validation, "The supplied username is already in use.");
        
        var emailIsUniqueResult = await _validateUniqueness.IsEmailAlreadyExists(email, token);
        if (emailIsUniqueResult.IsSuccess && emailIsUniqueResult.Value)
            return Result.Failure<Account>(ErrorKind.Validation, "The supplied email is already in use.");

        var hashedPassword = HashedPassword.Create(password, _passwordHasher);
        return Result.Success(Account.Create(username, email, hashedPassword, profile));
    }
}