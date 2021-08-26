using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class PspUserNormalDto : PspUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressDto Address { get; set; }
        public string AddressObsolete { get; set; }
        public DateTime? Birthday { get; set; }
        public string Birthplace { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public string ProofOfIdentity { get; set; }
        public string ProofOfAddress { get; set; }
    }
}