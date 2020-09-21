using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class SetRefundTransferStatusCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-transfer-refund-status";

        [JsonConstructor]
        public SetRefundTransferStatusCommand(RequestUser requestUser, PspEventKind kind, string identifier, DateTimeOffset executedOn)
            : base(requestUser, kind, identifier, executedOn)
        {
        }
    }
}
