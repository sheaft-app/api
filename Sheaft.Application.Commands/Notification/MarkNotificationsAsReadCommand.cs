using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class MarkUserNotificationsAsReadCommand : Command<bool>
    {
        public MarkUserNotificationsAsReadCommand(RequestUser user) : base(user)
        {
        }

        public DateTimeOffset ReadBefore { get; set; }
    }
}