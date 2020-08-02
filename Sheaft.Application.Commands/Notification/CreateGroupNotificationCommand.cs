using System;

namespace Sheaft.Application.Commands
{
    public class CreateGroupNotificationCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "creategroupnotification";

        public CreateGroupNotificationCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}