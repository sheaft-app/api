using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckPayinRefundsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayinRefundsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }
}
