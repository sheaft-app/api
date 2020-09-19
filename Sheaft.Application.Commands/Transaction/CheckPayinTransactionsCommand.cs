using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckPayinTransactionsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayinTransactionsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
