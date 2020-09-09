using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{

    public class SetCompanyLegalsInput
    {
        public Guid Id { get; set; }
        public LegalKind Legal { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public string Nationality { get; set; }
        public string CountryOfResidence { get; set; }
        public AddressInput LegalAddress { get; set; }
        public AddressInput BillingAddress { get; set; }
    }
}