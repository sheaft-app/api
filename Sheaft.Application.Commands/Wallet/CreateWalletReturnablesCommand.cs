using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateWalletReturnablesCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateWalletReturnablesCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
