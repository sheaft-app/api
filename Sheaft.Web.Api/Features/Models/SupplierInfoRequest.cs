using Sheaft.Application.Models;

namespace Sheaft.Web.Api;

#pragma warning disable CS8604
public record SupplierInfoRequest(string TradeName, string CorporateName, string Siret, string Email, string Phone,
    AddressDto LegalAddress, NamedAddressDto? ShippingAddress, NamedAddressDto? BillingAddress);
#pragma warning restore CS8604