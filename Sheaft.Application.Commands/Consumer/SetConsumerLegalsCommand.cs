using System;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class SetConsumerLegalsCommand : Command<bool>
    {
        [JsonConstructor]
        public SetConsumerLegalsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressInput Address { get; set; }
        public AddressInput BillingAddress { get; set; }
    }
}
