using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class UboDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public AddressDto Address { get; set; }
        public BirthAddressDto BirthPlace { get; set; }
    }
}