using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{

    public class SetCompanyLegalsInput
    {
        public Guid Id { get; set; }
        public LegalKind Legal { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressInput LegalAddress { get; set; }
        public AddressInput BillingAddress { get; set; }
    }
}