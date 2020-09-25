using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckPayoutsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayoutsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
