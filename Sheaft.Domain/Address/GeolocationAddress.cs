namespace Sheaft.Domain.Address;

public record GeolocationAddress(string Line1, string? Line2, string Postcode, string City, Location Location): Address(Line1, Line2, Postcode, City);