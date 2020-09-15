using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class OwnerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressDto Address { get; set; }
    }
}