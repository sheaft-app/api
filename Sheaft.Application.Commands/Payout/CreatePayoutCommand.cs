using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CreatePayoutCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePayoutCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
        public List<Guid> TransferIds { get; set; }
    }
}
