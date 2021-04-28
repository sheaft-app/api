using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class OwnerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressViewModel Address { get; set; }
    }
}
