using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class ExpireTransferRefundCommand : Command<bool>
    {
        [JsonConstructor]
        public ExpireTransferRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid TransferRefundId { get; set; }
    }
}
