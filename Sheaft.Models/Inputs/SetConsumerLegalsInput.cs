using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{

    public class SetConsumerLegalsInput
    {
        public Guid Id { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public string Nationality { get; set; }
        public string CountryOfResidence { get; set; }
        public AddressInput Address { get; set; }
        public AddressInput BillingAddress { get; set; }

    }
}