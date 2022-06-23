﻿using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Application.SupplierManagement;

public record ConfigureAccountAsSupplierCommand(string TradeName, string CorporateName, string Siret, string Email,
    string Phone, AddressDto LegalAddress, NamedAddressDto? ShippingAddress,
    NamedAddressDto? BillingAddress) : Command<Result<string>>;

public class ConfigureAccountAsSupplierHandler : ICommandHandler<ConfigureAccountAsSupplierCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidateSupplierRegistration _validateSupplierRegistration;

    public ConfigureAccountAsSupplierHandler(
        IUnitOfWork uow,
        IValidateSupplierRegistration validateSupplierRegistration)
    {
        _uow = uow;
        _validateSupplierRegistration = validateSupplierRegistration;
    }

    public async Task<Result<string>> Handle(ConfigureAccountAsSupplierCommand request, CancellationToken token)
    {
        var legalAddress = new LegalAddress(request.LegalAddress.Street, request.LegalAddress.Complement,
            request.LegalAddress.Postcode, request.LegalAddress.City);

        var shippingAddress = request.ShippingAddress != null
            ? new ShippingAddress(request.ShippingAddress.Name ?? request.TradeName,
                new EmailAddress(request.ShippingAddress.Email ?? request.Email), request.ShippingAddress.Street,
                request.ShippingAddress.Complement,
                request.ShippingAddress.Postcode, request.ShippingAddress.City)
            : null;

        var billingAddress = request.BillingAddress != null
            ? new BillingAddress(request.BillingAddress.Name ?? request.TradeName,
                new EmailAddress(request.BillingAddress.Email ?? request.Email), request.BillingAddress.Street,
                request.BillingAddress.Complement,
                request.BillingAddress.Postcode, request.BillingAddress.City)
            : null;

        var requireSupplierRegistrationResult =
            await _validateSupplierRegistration.CanRegisterAccount(request.RequestUser.AccountId, token);

        if (requireSupplierRegistrationResult.IsFailure)
            return Result.Failure<string>(requireSupplierRegistrationResult);

        if (!requireSupplierRegistrationResult.Value)
            return Result.Failure<string>(ErrorKind.BadRequest, "supplier.already.exists");

        var supplier = new Supplier(
            new TradeName(request.TradeName),
            new EmailAddress(request.Email),
            new PhoneNumber(request.Phone),
            new Legal(
                new CorporateName(request.CorporateName),
                new Siret(request.Siret), legalAddress),
            request.RequestUser.AccountId,
            shippingAddress,
            billingAddress);

        _uow.Suppliers.Add(supplier);
        var result = await _uow.Save(token);
        if (result.IsFailure)
            return Result.Failure<string>(result);

        return Result.Success(supplier.Id.Value);
    }
}