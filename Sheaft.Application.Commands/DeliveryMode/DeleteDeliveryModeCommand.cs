using System;

namespace Sheaft.Application.Commands
{
    public class DeleteDeliveryModeCommand : Command<bool>
    {
        public DeleteDeliveryModeCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
