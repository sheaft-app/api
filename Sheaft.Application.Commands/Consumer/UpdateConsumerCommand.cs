using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{
    public class UpdateConsumerCommand : ConsumerCommand<bool>
    {
        [JsonConstructor]
        public UpdateConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public AddressInput Address { get; set; }
        public AddressInput BillingAddress { get; set; }
        public bool Anonymous { get; set; }
    }
}
