using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class ExpireTransferCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-expire-transfer";

        [JsonConstructor]
        public ExpireTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransferId { get; set; }
    }
}
