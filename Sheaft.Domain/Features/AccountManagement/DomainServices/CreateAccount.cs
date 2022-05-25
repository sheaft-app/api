namespace Sheaft.Domain.AccountManagement;

public interface ICreateAccount
{
    Task<Result<Account>> Create(Username username, EmailAddress email, NewPassword password, Firstname firstname, Lastname lastname, CancellationToken token);
}

internal class CreateAccount : ICreateAccount
{
    private readonly IUniquenessValidator _uniquenessValidator;
    private readonly IPasswordHasher _passwordHasher;

    public CreateAccount(IUniquenessValidator uniquenessValidator, IPasswordHasher passwordHasher)
    {
        _uniquenessValidator = uniquenessValidator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Account>> Create(Username username, EmailAddress email, NewPassword password, Firstname firstname, Lastname lastname, CancellationToken token)
    {
        var usernameIsUniqueResult = await _uniquenessValidator.UsernameAlreadyExists(username, token);
        if (usernameIsUniqueResult.IsSuccess && usernameIsUniqueResult.Value)
            return Result.Failure<Account>(ErrorKind.Validation, "The supplied username is already in use.");
        
        var emailIsUniqueResult = await _uniquenessValidator.EmailAlreadyExists(email, token);
        if (emailIsUniqueResult.IsSuccess && emailIsUniqueResult.Value)
            return Result.Failure<Account>(ErrorKind.Validation, "The supplied email is already in use.");

        var hashedPassword = HashedPassword.Create(password, _passwordHasher);
        return Result.Success(new Account(username, email, hashedPassword, firstname, lastname));
    }
}