using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class CreateConsumerLegalDto 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressDto Address { get; set; }
        public Guid UserId { get; set; }
    }
}