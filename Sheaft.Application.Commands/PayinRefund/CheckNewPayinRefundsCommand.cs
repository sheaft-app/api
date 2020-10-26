using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckNewPayinRefundsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckNewPayinRefundsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }
}
