using FluentValidation;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Application.AccountManagement;

public record ResetPasswordCommand(string Username, string ResetToken, string Password, string Confirm) : ICommand<Result>;

internal class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(c => c.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(c => c.ResetToken).NotEmpty().WithMessage("ResetToken is required");
        
        RuleFor(c => c.Password).NotEmpty().WithMessage("New password is required");
        RuleFor(c => c.Confirm).NotEmpty().WithMessage("Confirmation is required");
        RuleFor(c => c.Confirm).Equal(c => c.Password).WithMessage("Confirmation is different.");
    }
}

internal class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordHandler(
        IUnitOfWork uow,
        IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken token)
    {
        var accountResult = await _uow.Accounts.Get(new Username(request.Username), token);
        if (accountResult.IsFailure)
            return accountResult;

        var account = accountResult.Value;
        var resetPasswordResult = account.ResetPassword(request.ResetToken, new NewPassword(request.Password, request.Confirm), _passwordHasher);
        if (resetPasswordResult.IsFailure)
            return Result.Failure(resetPasswordResult);

        _uow.Accounts.Update(account);
        return await _uow.Save(token);
    }
}