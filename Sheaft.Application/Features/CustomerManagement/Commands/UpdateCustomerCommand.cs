using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.CustomerManagement;

public record UpdateCustomerCommand(CustomerId Identifier, string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, AddressDto? DeliveryAddress) : ICommand<Result>;

public class UpdateCustomerHandler : ICommandHandler<UpdateCustomerCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateCustomerHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken token)
    {
        var legalAddress = new LegalAddress(request.LegalAddress.Street, request.LegalAddress.Complement,
            request.LegalAddress.Postcode, request.LegalAddress.City);

        var address = request.DeliveryAddress ?? request.LegalAddress;
        var deliveryAddress = new DeliveryAddress(address.Street, address.Complement,
            address.Postcode, address.City);

        var customerResult = await _uow.Customers.Get(request.Identifier, token);
        if (customerResult.IsFailure)
            return Result.Failure(customerResult);

        var customer = customerResult.Value;
        var setInfoResult = customer.SetInfo(new TradeName(request.TradeName), new EmailAddress(request.Email), new PhoneNumber(request.Phone), deliveryAddress);
        if (setInfoResult.IsFailure)
            return setInfoResult;
        
        var setLegalResult = customer.SetLegal(new CorporateName(request.CorporateName), new Siret(request.Siret), legalAddress);
        if (setLegalResult.IsFailure)
            return setLegalResult;

        _uow.Customers.Update(customer);
        return await _uow.Save(token);
    }
}
