using System;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class SetConsumerLegalsCommand : Command<bool>
    {
        [JsonConstructor]
        public SetConsumerLegalsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public OwnerInput Owner { get; set; }
    }
}
