using System;

namespace Sheaft.Application.Commands
{
    public class MarkUserNotificationAsReadCommand : Command<bool>
    {
        public MarkUserNotificationAsReadCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}