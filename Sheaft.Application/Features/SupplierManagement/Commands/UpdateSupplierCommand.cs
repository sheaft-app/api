using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Application.SupplierManagement;

public record UpdateSupplierCommand(SupplierId Identifier, string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? ShippingAddress) : ICommand<Result>;

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

        var address = request.ShippingAddress ?? request.LegalAddress;
        var shippingAddress = new ShippingAddress(address.Street, address.Complement,
            address.Postcode, address.City);

        var supplierResult = await _uow.Suppliers.Get(request.Identifier, token);
        if (supplierResult.IsFailure)
            return Result.Failure(supplierResult);

        var supplier = supplierResult.Value;
        supplier.SetInfo(new TradeName(request.TradeName), new EmailAddress(request.Email), new PhoneNumber(request.Phone), shippingAddress);
        supplier.SetLegal(new CorporateName(request.CorporateName), new Siret(request.Siret), legalAddress);
        
        _uow.Suppliers.Update(supplier);
        return await _uow.Save(token);
    }
}
