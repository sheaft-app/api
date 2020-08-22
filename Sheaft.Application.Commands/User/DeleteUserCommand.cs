using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteUserCommand : Command<bool>
    {
        public DeleteUserCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
