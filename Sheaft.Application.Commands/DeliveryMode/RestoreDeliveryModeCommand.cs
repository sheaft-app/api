using System;

namespace Sheaft.Application.Commands
{
    public class RestoreDeliveryModeCommand : Command<bool>
    {
        public RestoreDeliveryModeCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
