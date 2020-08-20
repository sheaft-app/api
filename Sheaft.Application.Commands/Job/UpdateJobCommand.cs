using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class UpdateJobCommand : Command<bool>
    {
        public UpdateJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
