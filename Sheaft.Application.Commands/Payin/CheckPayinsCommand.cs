using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckPayinsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayinsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
