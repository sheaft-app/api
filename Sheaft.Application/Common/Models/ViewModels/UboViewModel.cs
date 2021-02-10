using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class UboViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public AddressViewModel Address { get; set; }
        public BirthAddressViewModel BirthPlace { get; set; }
    }
}
