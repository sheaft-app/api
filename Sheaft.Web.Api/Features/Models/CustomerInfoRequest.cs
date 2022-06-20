using Sheaft.Application.Models;

namespace Sheaft.Web.Api;

#pragma warning disable CS8604
public record CustomerInfoRequest(string TradeName, string CorporateName, string Siret, string Email, string Phone,
    AddressDto LegalAddress, NamedAddressDto? DeliveryAddress, NamedAddressDto? BillingAddress);
#pragma warning restore CS8604