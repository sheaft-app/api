using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckTransferRefundCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-transfer-refund";

        [JsonConstructor]
        public CheckTransferRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid TransferRefundId { get; set; }
    }
}
