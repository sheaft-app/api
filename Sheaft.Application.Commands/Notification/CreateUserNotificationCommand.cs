using System;

namespace Sheaft.Application.Commands
{
    public class CreateUserNotificationCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "createusernotification";

        public CreateUserNotificationCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}