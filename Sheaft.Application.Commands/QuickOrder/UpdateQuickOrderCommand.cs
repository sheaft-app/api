using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateQuickOrderCommand : Command<bool>
    {
        public UpdateQuickOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
