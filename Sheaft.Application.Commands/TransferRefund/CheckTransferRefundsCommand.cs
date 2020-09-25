using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckTransferRefundsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckTransferRefundsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }
}
