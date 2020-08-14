using System;

namespace Sheaft.Application.Commands
{
    public class RemoveUserDataCommand : Command<string>
    {
        public const string QUEUE_NAME = "command-users-removedata";

        public RemoveUserDataCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
