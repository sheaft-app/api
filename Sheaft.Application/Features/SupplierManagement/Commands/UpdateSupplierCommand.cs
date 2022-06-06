using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.SupplierManagement;

public record UpdateSupplierCommand(SupplierId Identifier, string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, NamedAddressDto? ShippingAddress, NamedAddressDto? BillingAddress) : ICommand<Result>;

public class UpdateSupplierHandler : ICommandHandler<UpdateSupplierCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateSupplierHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateSupplierCommand request, CancellationToken token)
    {
        var legalAddress = new LegalAddress(request.LegalAddress.Street, request.LegalAddress.Complement,
            request.LegalAddress.Postcode, request.LegalAddress.City);

        var supplierResult = await _uow.Suppliers.Get(request.Identifier, token);
        if (supplierResult.IsFailure)
            return Result.Failure(supplierResult);

        var supplier = supplierResult.Value;
        supplier.SetInfo(new TradeName(request.TradeName), new EmailAddress(request.Email), new PhoneNumber(request.Phone));
        supplier.SetLegal(new CorporateName(request.CorporateName), new Siret(request.Siret), legalAddress);
        
        if (request.BillingAddress != null)
            supplier.SetBillingAddress(new BillingAddress(
                request.BillingAddress.Name,
                new EmailAddress(request.BillingAddress.Email),
                request.BillingAddress.Street,
                request.BillingAddress.Complement,
                request.BillingAddress.Postcode,
                request.BillingAddress.City));

        if (request.ShippingAddress != null)
            supplier.SetShippingAddress(new ShippingAddress(
                request.ShippingAddress.Name,
                new EmailAddress(request.ShippingAddress.Email),
                request.ShippingAddress.Street,
                request.ShippingAddress.Complement,
                request.ShippingAddress.Postcode,
                request.ShippingAddress.City));

        _uow.Suppliers.Update(supplier);
        return await _uow.Save(token);
    }
}
