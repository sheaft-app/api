using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckForNewPayoutsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckForNewPayoutsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
