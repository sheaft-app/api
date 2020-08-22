using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class MarkUserNotificationAsReadCommand : Command<bool>
    {
        public MarkUserNotificationAsReadCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}