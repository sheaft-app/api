using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateUserCommand : UserCommand<bool>
    {
        public UpdateUserCommand(RequestUser user) : base(user)
        {
        }

        public Guid? DepartmentId { get; set; }
        public bool Anonymous { get; set; }
    }
}
