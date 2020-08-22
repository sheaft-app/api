using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateJobCommand : Command<bool>
    {
        public UpdateJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
