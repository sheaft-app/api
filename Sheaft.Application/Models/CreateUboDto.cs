using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class CreateUboDto
    {
        public Guid DeclarationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public AddressDto Address { get; set; }
        public BirthAddressDto BirthPlace { get; set; }
    }
}