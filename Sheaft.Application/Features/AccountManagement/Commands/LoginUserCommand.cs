using FluentValidation;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Application.AccountManagement;

public record LoginUserCommand(string Username, string Password) : ICommand<Result<AuthenticationTokenDto>>;

internal class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(c => c.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(c => c.Password).NotEmpty().WithMessage("Password is required");
    }
}

internal class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<AuthenticationTokenDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISecurityTokensProvider _securityTokensProvider;

    public LoginUserHandler(
        IUnitOfWork uow,
        IPasswordHasher passwordHasher,
        ISecurityTokensProvider securityTokensProvider)
    {
        _uow = uow;
        _passwordHasher = passwordHasher;
        _securityTokensProvider = securityTokensProvider;
    }

    public async Task<Result<AuthenticationTokenDto>> Handle(LoginUserCommand request, CancellationToken token)
    {
        var accountResult = await _uow.Accounts.Get(new Username(request.Username), token);
        if (accountResult.IsFailure)
            return Result.Failure<AuthenticationTokenDto>(accountResult);

        var account = accountResult.Value;
        var loginResult = account.Login(request.Password, _passwordHasher, _securityTokensProvider);
        if (loginResult.IsFailure)
            return Result.Failure<AuthenticationTokenDto>(loginResult);

        _uow.Accounts.Update(account);
        var result = await _uow.Save(token);
        
        return result.IsSuccess
            ? Result.Success(new AuthenticationTokenDto(loginResult.Value.AccessToken, loginResult.Value.RefreshToken, loginResult.Value.TokenType, loginResult.Value.ExpiresIn))
            : Result.Failure<AuthenticationTokenDto>(result);
    }
}