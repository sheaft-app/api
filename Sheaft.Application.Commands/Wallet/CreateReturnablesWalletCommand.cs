using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateReturnablesWalletCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateReturnablesWalletCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
