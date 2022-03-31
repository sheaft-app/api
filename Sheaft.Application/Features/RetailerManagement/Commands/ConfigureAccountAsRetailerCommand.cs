using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.RetailerManagement;

namespace Sheaft.Application.RetailerManagement;

public record ConfigureAccountAsRetailerCommand(string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? DeliveryAddress, AccountId AccountIdentifier) : ICommand<Result<string>>;

public class ConfigureAccountAsRetailerHandler : ICommandHandler<ConfigureAccountAsRetailerCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidateRetailerRegistration _validateRetailerRegistration;

    public ConfigureAccountAsRetailerHandler(
        IUnitOfWork uow, 
        IValidateRetailerRegistration validateRetailerRegistration)
    {
        _uow = uow;
        _validateRetailerRegistration = validateRetailerRegistration;
    }

    public async Task<Result<string>> Handle(ConfigureAccountAsRetailerCommand request, CancellationToken token)
    {
        var legalAddress = new LegalAddress(request.LegalAddress.Street, request.LegalAddress.Complement,
            request.LegalAddress.Postcode, request.LegalAddress.City);

        var deliveryAddress = request.DeliveryAddress != null
            ? new DeliveryAddress(request.DeliveryAddress.Street, request.DeliveryAddress.Complement,
                request.DeliveryAddress.Postcode, request.DeliveryAddress.City)
            : null;

        var requireRetailerRegistrationResult =
            await _validateRetailerRegistration.CanRegisterAccount(request.AccountIdentifier, token);

        if (requireRetailerRegistrationResult.IsFailure)
            return Result.Failure<string>(requireRetailerRegistrationResult);

        if (!requireRetailerRegistrationResult.Value)
            return Result.Failure<string>(ErrorKind.BadRequest, "supplier.already.exists");

        var retailer = new Retailer(
            new TradeName(request.TradeName), 
            new EmailAddress(request.Email), 
            new PhoneNumber(request.Phone), 
            new Legal(
                new CorporateName(request.CorporateName),
                new Siret(request.Siret), legalAddress),
            deliveryAddress, 
            request.AccountIdentifier);
        
        _uow.Retailers.Add(retailer);
        await _uow.Save(token);
        
        return Result.Success(retailer.Identifier.Value);
    }
}
