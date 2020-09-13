using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{
    public class UboInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset Birthdate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public AddressInput Address { get; set; }
        public BirthAddressInput BirthAddress { get; set; }
    }
}