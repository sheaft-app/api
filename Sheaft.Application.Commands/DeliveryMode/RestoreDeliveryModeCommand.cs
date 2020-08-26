using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestoreDeliveryModeCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
