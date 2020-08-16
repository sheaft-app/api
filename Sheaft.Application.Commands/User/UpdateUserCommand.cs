using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class UpdateUserCommand : UserCommand<bool>
    {
        public UpdateUserCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid? DepartmentId { get; set; }
        public bool Anonymous { get; set; }
    }
}
