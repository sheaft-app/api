using FluentValidation;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Application.AccountManagement;

public record RegisterAccountCommand
    (string Email, string Password, string Confirm, string Firstname, string Lastname) : ICommand<Result<string>>;

internal class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(c => c.Email).Matches(EmailAddress.EMAIL_REGEX).WithMessage("Email is invalid");
        
        RuleFor(c => c.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(c => c.Confirm).NotEmpty().WithMessage("Confirmation is required");
        RuleFor(c => c.Confirm).Equal(c => c.Password).WithMessage("Confirmation is different.");
        
        RuleFor(c => c.Firstname).NotEmpty().WithMessage("Firstname is required");
        RuleFor(c => c.Lastname).NotEmpty().WithMessage("Lastname is required");
    }
}

internal class RegisterAccountHandler : ICommandHandler<RegisterAccountCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICreateAccount _createAccount;

    public RegisterAccountHandler(
        IUnitOfWork uow,
        ICreateAccount createAccount)
    {
        _uow = uow;
        _createAccount = createAccount;
    }

    public async Task<Result<string>> Handle(RegisterAccountCommand request, CancellationToken token)
    {
        var email = new EmailAddress(request.Email);
        var accountResult = await _createAccount.Create(
            new Username(request.Email),
            email,
            new NewPassword(request.Password, request.Confirm),
            new Firstname(request.Firstname),
            new Lastname(request.Lastname),
            token);

        if (accountResult.IsFailure)
            return Result.Failure<string>(accountResult);
        
        _uow.Accounts.Add(accountResult.Value);
        var result = await _uow.Save(token);
        
        return Result.SuccessIf(result, accountResult.Value.Identifier.Value);
    }
}