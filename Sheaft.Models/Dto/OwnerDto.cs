using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class OwnerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public LegalKind Legal { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressDto Address { get; set; }
    }
}