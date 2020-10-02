using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckNewPayoutsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckNewPayoutsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
