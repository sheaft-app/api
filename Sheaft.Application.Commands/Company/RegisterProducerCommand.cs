using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class RegisterProducerCommand : Command<Guid>
    {
        [JsonConstructor]
        public RegisterProducerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string VatIdentifier { get; set; }
        public string Picture { get; set; }
        public string Siret { get; set; }
        public AddressInput BillingAddress { get; set; }
        public AddressInput Address { get; set; }
        public RegisterOwnerInput Owner { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}
