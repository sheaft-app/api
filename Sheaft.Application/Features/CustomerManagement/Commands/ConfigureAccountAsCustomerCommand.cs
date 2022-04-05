using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.CustomerManagement;

public record ConfigureAccountAsCustomerCommand(string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? DeliveryAddress, AccountId AccountIdentifier) : ICommand<Result<string>>;

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
            ? new DeliveryAddress(request.DeliveryAddress.Street, request.DeliveryAddress.Complement,
                request.DeliveryAddress.Postcode, request.DeliveryAddress.City)
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
            deliveryAddress, 
            request.AccountIdentifier);
        
        _uow.Customers.Add(customer);
        var result = await _uow.Save(token);
        if (result.IsFailure)
            return Result.Failure<string>(result);
        
        return Result.Success(customer.Identifier.Value);
    }
}
