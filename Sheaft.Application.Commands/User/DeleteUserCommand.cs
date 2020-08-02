using System;

namespace Sheaft.Application.Commands
{
    public class DeleteUserCommand : Command<bool>
    {
        public DeleteUserCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
