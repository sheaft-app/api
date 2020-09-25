using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckTransferCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-transfer";

        [JsonConstructor]
        public CheckTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransferId { get; set; }
    }
}
