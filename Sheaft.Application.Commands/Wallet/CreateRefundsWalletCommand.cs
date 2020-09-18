using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateRefundsWalletCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateRefundsWalletCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
