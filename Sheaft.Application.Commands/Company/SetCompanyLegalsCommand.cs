using System;
using Sheaft.Interop.Enums;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class SetCompanyLegalsCommand : Command<bool>
    {
        [JsonConstructor]
        public SetCompanyLegalsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public LegalKind Legal { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressInput LegalAddress { get; set; }
        public AddressInput BillingAddress { get; set; }
    }
}
