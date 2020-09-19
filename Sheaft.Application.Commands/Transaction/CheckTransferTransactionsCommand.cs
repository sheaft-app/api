using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckTransferTransactionsCommand : Command<Guid>
    {
        [JsonConstructor]
        public CheckTransferTransactionsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
