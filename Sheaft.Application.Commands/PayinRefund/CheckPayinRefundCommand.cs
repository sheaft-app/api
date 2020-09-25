using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckPayinRefundCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-payin-refund";

        [JsonConstructor]
        public CheckPayinRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PayinRefundId { get; set; }
    }
}
