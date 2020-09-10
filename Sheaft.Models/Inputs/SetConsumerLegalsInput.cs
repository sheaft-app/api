using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{

    public class SetConsumerLegalsInput
    {
        public Guid Id { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressInput Address { get; set; }
        public AddressInput BillingAddress { get; set; }

    }
}