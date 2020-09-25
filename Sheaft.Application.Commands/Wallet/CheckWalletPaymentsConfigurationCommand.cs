using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CheckWalletPaymentsConfigurationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckWalletPaymentsConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
