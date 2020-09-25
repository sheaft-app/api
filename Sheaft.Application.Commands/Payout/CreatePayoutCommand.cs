using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CreatePayoutCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "command-create-payout";

        [JsonConstructor]
        public CreatePayoutCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
        public IEnumerable<Guid> TransferIds { get; set; }
    }
}
