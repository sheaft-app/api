using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Domain;

namespace Sheaft.Application.AccountManagement;

public record ForgotPasswordCommand(string Email) : ICommand<Result>;

internal class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(c => c.Email).Matches(EmailAddress.EMAIL_REGEX).WithMessage("Email is invalid");
    }
}

internal class ForgotPasswordHandler : ICommandHandler<ForgotPasswordCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly ISecuritySettings _securitySettings;
    private readonly ILogger<ForgotPasswordHandler> _logger;

    public ForgotPasswordHandler(
        IUnitOfWork uow,
        ISecuritySettings securitySettings,
        ILogger<ForgotPasswordHandler> logger)
    {
        _uow = uow;
        _securitySettings = securitySettings;
        _logger = logger;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken token)
    {
        var accountResult = await _uow.Accounts.Find(new EmailAddress(request.Email), token);
        if (accountResult.IsFailure)
            return accountResult;
        
        if(accountResult.Value.HasNoValue)
        {
            _logger.LogInformation("{Email} requested a forgot password token, but email was not found", request.Email);
            return Result.Success();
        }

        var account = accountResult.Value.Value;
        account.ForgotPassword(DateTimeOffset.UtcNow, _securitySettings.ResetPasswordTokenValidityInHours);

        _uow.Accounts.Update(account);
        return await _uow.Save(token);
    }
}