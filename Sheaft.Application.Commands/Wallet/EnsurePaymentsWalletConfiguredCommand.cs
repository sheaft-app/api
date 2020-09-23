using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class EnsurePaymentsWalletConfiguredCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsurePaymentsWalletConfiguredCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
