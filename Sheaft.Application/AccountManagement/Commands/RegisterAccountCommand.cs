using FluentValidation;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Application.AccountManagement;

public record RegisterAccountCommand
    (string Email, string Password, string Confirm,
        string TradeName, string ContactEmail, string ContactPhone, 
        string CommercialName, string Siret,
        string AddressLine1, string AddressLine2, string AddressZipcode, string AddressCity,
        string Firstname, string Lastname) : ICommand<Result<string>>;

internal class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(c => c.Email).Matches(EmailAddress.EMAIL_REGEX).WithMessage("Email is invalid");
        
        RuleFor(c => c.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(c => c.Confirm).NotEmpty().WithMessage("Confirmation is required");
        RuleFor(c => c.Confirm).Equal(c => c.Password).WithMessage("Confirmation is different.");
        
        RuleFor(c => c.TradeName).NotEmpty().WithMessage("TradeName is required");
        RuleFor(c => c.CommercialName).NotEmpty().WithMessage("CommercialName is required");
        RuleFor(c => c.Siret).NotEmpty().WithMessage("Siret is required");
        
        RuleFor(c => c.ContactEmail).NotEmpty().WithMessage("ContactEmail is required");
        RuleFor(c => c.ContactEmail).Matches(EmailAddress.EMAIL_REGEX).WithMessage("ContactEmail is invalid");

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
        var profile = new Profile(
            new CompanyName(request.TradeName), 
            new EmailAddress(request.ContactEmail), 
            new PhoneNumber(request.ContactPhone), 
            new Legal(
                new LegalName(request.CommercialName), 
                new Siret(request.Siret), 
                new Address(request.AddressLine1, request.AddressLine2, request.AddressZipcode, request.AddressCity)), 
            new UserInfo(request.Firstname, request.Lastname));

        var email = new EmailAddress(request.Email);
        var accountResult = await _createAccount.Create(
            new Username(request.Email),
            email,
            new NewPassword(request.Password, request.Confirm),
            profile,
            token);

        if (accountResult.IsFailure)
            return Result.Failure<string>(accountResult);
        
        _uow.Add(accountResult.Value);
        _uow.Add(profile);
        
        var result = await _uow.Save(token);
        return Result.SuccessIf(result, accountResult.Value.Profile.Identifier.Value);
    }
}