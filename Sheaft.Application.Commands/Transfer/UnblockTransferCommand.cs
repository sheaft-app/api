using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UnblockTransferCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-unblock-transfer";

        [JsonConstructor]
        public UnblockTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransferId { get; set; }
    }
}
