using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.CustomerManagement;

public record UpdateCustomerCommand(CustomerId Identifier, string TradeName, string CorporateName, string Siret, string Email, string Phone, AddressDto LegalAddress, NamedAddressDto? DeliveryAddress, NamedAddressDto? BillingAddress) : ICommand<Result>;

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

        var customerResult = await _uow.Customers.Get(request.Identifier, token);
        if (customerResult.IsFailure)
            return Result.Failure(customerResult);

        var customer = customerResult.Value;
        
        customer.SetInfo(new TradeName(request.TradeName), new EmailAddress(request.Email), new PhoneNumber(request.Phone));
        customer.SetLegal(new CorporateName(request.CorporateName), new Siret(request.Siret), legalAddress);
        
        if (request.BillingAddress != null)
            customer.SetBillingAddress(new BillingAddress(
                request.BillingAddress.Name,
                new EmailAddress(request.BillingAddress.Email),
                request.BillingAddress.Street,
                request.BillingAddress.Complement,
                request.BillingAddress.Postcode,
                request.BillingAddress.City));

        if (request.DeliveryAddress != null)
            customer.SetDeliveryAddress(new DeliveryAddress(
                request.DeliveryAddress.Name,
                new EmailAddress(request.DeliveryAddress.Email),
                request.DeliveryAddress.Street,
                request.DeliveryAddress.Complement,
                request.DeliveryAddress.Postcode,
                request.DeliveryAddress.City));

        _uow.Customers.Update(customer);
        return await _uow.Save(token);
    }
}
