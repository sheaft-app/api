using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.CustomerManagement;

public record ConfigureAccountAsCustomerCommand(string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, NamedAddressDto? DeliveryAddress, NamedAddressDto? BillingAddress, AccountId AccountIdentifier) : ICommand<Result<string>>;

public class ConfigureAccountAsCustomerHandler : ICommandHandler<ConfigureAccountAsCustomerCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidateCustomerRegistration _validateCustomerRegistration;

    public ConfigureAccountAsCustomerHandler(
        IUnitOfWork uow, 
        IValidateCustomerRegistration validateCustomerRegistration)
    {
        _uow = uow;
        _validateCustomerRegistration = validateCustomerRegistration;
    }

    public async Task<Result<string>> Handle(ConfigureAccountAsCustomerCommand request, CancellationToken token)
    {
        var legalAddress = new LegalAddress(request.LegalAddress.Street, request.LegalAddress.Complement,
            request.LegalAddress.Postcode, request.LegalAddress.City);

        var deliveryAddress = request.DeliveryAddress != null
            ? new DeliveryAddress(request.DeliveryAddress.Name, new EmailAddress(request.DeliveryAddress.Email), request.DeliveryAddress.Street, request.DeliveryAddress.Complement,
                request.DeliveryAddress.Postcode, request.DeliveryAddress.City)
            : null;
        
        var billingAddress = request.BillingAddress != null
            ? new BillingAddress(request.BillingAddress.Name, new EmailAddress(request.BillingAddress.Email), request.BillingAddress.Street, request.BillingAddress.Complement,
                request.BillingAddress.Postcode, request.BillingAddress.City)
            : null;

        var requireCustomerRegistrationResult =
            await _validateCustomerRegistration.CanRegisterAccount(request.AccountIdentifier, token);

        if (requireCustomerRegistrationResult.IsFailure)
            return Result.Failure<string>(requireCustomerRegistrationResult);

        if (!requireCustomerRegistrationResult.Value)
            return Result.Failure<string>(ErrorKind.BadRequest, "supplier.already.exists");

        var customer = new Customer(
            new TradeName(request.TradeName), 
            new EmailAddress(request.Email), 
            new PhoneNumber(request.Phone), 
            new Legal(
                new CorporateName(request.CorporateName),
                new Siret(request.Siret), legalAddress), 
            request.AccountIdentifier,
            deliveryAddress,
            billingAddress);
        
        _uow.Customers.Add(customer);
        var result = await _uow.Save(token);
        if (result.IsFailure)
            return Result.Failure<string>(result);
        
        return Result.Success(customer.Identifier.Value);
    }
}
