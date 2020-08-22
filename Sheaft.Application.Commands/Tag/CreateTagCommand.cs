using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateTagCommand : Command<Guid>
    {
        public CreateTagCommand(RequestUser user) : base(user)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public TagKind Kind { get; set; }
        public string Image { get; set; }
    }
}
