using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class ExpirePayoutCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-expire-payout";

        [JsonConstructor]
        public ExpirePayoutCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }
}
