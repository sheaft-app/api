using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateWalletPaymentsCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateWalletPaymentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
