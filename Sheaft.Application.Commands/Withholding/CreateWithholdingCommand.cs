using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class CreateWithholdingCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateWithholdingCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
