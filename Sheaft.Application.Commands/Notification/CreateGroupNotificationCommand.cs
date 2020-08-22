using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateGroupNotificationCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "command-notify-group";

        public CreateGroupNotificationCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}