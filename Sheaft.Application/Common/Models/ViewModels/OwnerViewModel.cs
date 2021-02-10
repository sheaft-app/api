using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class OwnerViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressViewModel Address { get; set; }
    }
}
