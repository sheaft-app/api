using System;

namespace Sheaft.Application.Commands
{
    public class MarkUserNotificationsAsReadCommand : Command<bool>
    {
        public MarkUserNotificationsAsReadCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public DateTimeOffset ReadBefore { get; set; }
    }
}