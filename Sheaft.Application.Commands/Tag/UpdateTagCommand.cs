using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Application.Commands
{

    public class UpdateTagCommand : Command<bool>
    {
        public UpdateTagCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TagKind Kind { get; set; }
        public string Image { get; set; }
    }
}
