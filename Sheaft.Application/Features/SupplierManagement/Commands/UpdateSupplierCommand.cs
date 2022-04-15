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
        var setInfoResult = supplier.SetInfo(new TradeName(request.TradeName), new EmailAddress(request.Email), new PhoneNumber(request.Phone));
        if (setInfoResult.IsFailure)
            return setInfoResult;
        
        var setLegalResult = supplier.SetLegal(new CorporateName(request.CorporateName), new Siret(request.Siret), legalAddress);
        if (setLegalResult.IsFailure)
            return setLegalResult;
        
        if (request.BillingAddress != null){
            var billingResult = supplier.SetBillingAddress(new BillingAddress(
                request.BillingAddress.Name,
                new EmailAddress(request.BillingAddress.Email), 
                request.BillingAddress.Street, 
                request.BillingAddress.Complement, 
                request.BillingAddress.Postcode, 
                request.BillingAddress.City));
            
            if (billingResult.IsFailure)
                return billingResult;
        }

        if (request.ShippingAddress != null)
        {
            var shippingResult = supplier.SetShippingAddress(new ShippingAddress(
                request.ShippingAddress.Name,
                new EmailAddress(request.ShippingAddress.Email),
                request.ShippingAddress.Street,
                request.ShippingAddress.Complement,
                request.ShippingAddress.Postcode,
                request.ShippingAddress.City));
            
            if (shippingResult.IsFailure)
                return shippingResult;
        }
        
        _uow.Suppliers.Update(supplier);
        return await _uow.Save(token);
    }
}
