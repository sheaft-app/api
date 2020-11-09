using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class EnsureBankAccountValidatedCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureBankAccountValidatedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
