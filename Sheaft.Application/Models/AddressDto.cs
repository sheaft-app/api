namespace Sheaft.Application.Models;

public record AddressDto(string Street, string? Complement, string Postcode, string City);

public record NamedAddressDto(string Name, string Email, string Street, string? Complement, string Postcode, string City);