using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckPayoutCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-payout";

        [JsonConstructor]
        public CheckPayoutCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }
}
