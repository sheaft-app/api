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
        public string Nationality { get; set; }
        public string CountryOfResidence { get; set; }
        public AddressDto Address { get; set; }
    }
}