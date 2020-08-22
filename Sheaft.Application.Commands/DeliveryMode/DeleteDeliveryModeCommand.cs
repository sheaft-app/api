using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteDeliveryModeCommand : Command<bool>
    {
        public DeleteDeliveryModeCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
