using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class ProcessWithholdingsCommand : Command<bool>
    {
        [JsonConstructor]
        public ProcessWithholdingsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }
}
