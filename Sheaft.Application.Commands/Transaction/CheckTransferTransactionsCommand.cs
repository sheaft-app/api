using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckTransferTransactionsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckTransferTransactionsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
