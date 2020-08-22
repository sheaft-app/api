using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestoreDeliveryModeCommand : Command<bool>
    {
        public RestoreDeliveryModeCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
