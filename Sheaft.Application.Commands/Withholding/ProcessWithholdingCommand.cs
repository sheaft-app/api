using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class ProcessWithholdingCommand : Command<TransactionStatus>
    {
        [JsonConstructor]
        public ProcessWithholdingCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid WithholdingId { get; set; }
    }
}
