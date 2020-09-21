using Sheaft.Domain.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class RestoreRewardCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreRewardCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
