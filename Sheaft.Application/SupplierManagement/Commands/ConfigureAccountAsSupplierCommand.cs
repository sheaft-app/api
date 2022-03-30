using Microsoft.Extensions.Logging;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Application.SupplierManagement;

public record ConfigureAccountAsSupplierCommand(string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? ShippingAddress, AccountId AccountIdentifier) : ICommand<Result>;

public class ConfigureAccountAsSupplierHandler : ICommandHandler<ConfigureAccountAsSupplierCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly ISupplierRegistrationValidator _supplierRegistrationValidator;
    private readonly ILogger<ConfigureAccountAsSupplierHandler> _logger;

    public ConfigureAccountAsSupplierHandler(
        IUnitOfWork uow, 
        ISupplierRegistrationValidator supplierRegistrationValidator,
        ILogger<ConfigureAccountAsSupplierHandler> logger)
    {
        _uow = uow;
        _supplierRegistrationValidator = supplierRegistrationValidator;
        _logger = logger;
    }

    public async Task<Result> Handle(ConfigureAccountAsSupplierCommand request, CancellationToken token)
    {
        var legalAddress = new Address(request.LegalAddress.Street, request.LegalAddress.Complement,
            request.LegalAddress.Postcode, request.LegalAddress.City);

        var shippingAddress = request.ShippingAddress != null
            ? new Address(request.ShippingAddress.Street, request.ShippingAddress.Complement,
                request.ShippingAddress.Postcode, request.ShippingAddress.City)
            : null;

        var requireSupplierRegistrationResult =
            await _supplierRegistrationValidator.CanRegisterAccount(request.AccountIdentifier, token);

        if (requireSupplierRegistrationResult.IsFailure)
            return Result.Failure(requireSupplierRegistrationResult);

        if (!requireSupplierRegistrationResult.Value)
            return Result.Failure(ErrorKind.BadRequest, "supplier.already.exists");
        
        _uow.Suppliers.Add(new Supplier(
            new TradeName(request.TradeName), 
            new EmailAddress(request.Email), 
            new PhoneNumber(request.Phone), 
            new Legal(
                new CorporateName(request.CorporateName),
                new Siret(request.Siret), legalAddress),
            shippingAddress, 
            request.AccountIdentifier));
        
        await _uow.Save(token);
        return Result.Success();
    }
}
