using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreatePaymentsWalletCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePaymentsWalletCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
