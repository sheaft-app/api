using Sheaft.Core;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class UpdateTagCommand : Command<bool>
    {
        public UpdateTagCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TagKind Kind { get; set; }
        public string Image { get; set; }
    }
}
