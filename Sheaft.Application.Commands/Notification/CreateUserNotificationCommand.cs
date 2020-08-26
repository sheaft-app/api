using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateUserNotificationCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "command-notify-user";

        [JsonConstructor]
        public CreateUserNotificationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}