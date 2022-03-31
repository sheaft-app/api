using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.RetailerManagement;

namespace Sheaft.Application.RetailerManagement;

public record UpdateRetailerCommand(RetailerId Identifier, string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? DeliveryAddress) : ICommand<Result>;

public class UpdateRetailerHandler : ICommandHandler<UpdateRetailerCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateRetailerHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateRetailerCommand request, CancellationToken token)
    {
        var legalAddress = new LegalAddress(request.LegalAddress.Street, request.LegalAddress.Complement,
            request.LegalAddress.Postcode, request.LegalAddress.City);

        var address = request.DeliveryAddress ?? request.LegalAddress;
        var deliveryAddress = new DeliveryAddress(address.Street, address.Complement,
            address.Postcode, address.City);

        var retailerResult = await _uow.Retailers.Get(request.Identifier, token);
        if (retailerResult.IsFailure)
            return Result.Failure(retailerResult);

        var retailer = retailerResult.Value;
        retailer.SetInfo(new TradeName(request.TradeName), new EmailAddress(request.Email), new PhoneNumber(request.Phone), deliveryAddress);
        retailer.SetLegal(new CorporateName(request.CorporateName), new Siret(request.Siret), legalAddress);
        
        _uow.Retailers.Update(retailer);
        await _uow.Save(token);
        
        return Result.Success();
    }
}
