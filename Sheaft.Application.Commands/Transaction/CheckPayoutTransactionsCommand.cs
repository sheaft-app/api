using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckPayoutTransactionsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayoutTransactionsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
